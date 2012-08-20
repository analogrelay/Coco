using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coco.Controls
{
    [TemplatePart(Name = "PART_Host")]
    [TemplatePart(Name = "PART_Debug_InsertMarker")]
    public class Console : Control, IConsoleHost
    {
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(IConsoleModel), typeof(Console), new PropertyMetadata(ConsoleModelChanged));
        public static readonly DependencyProperty DisplayDebugMarkersProperty = DependencyProperty.Register(
            "DisplayDebugMarkers", typeof(bool), typeof(Console));
        public static readonly DependencyProperty ColorMapProperty = DependencyProperty.Register(
            "ColorMap", typeof(IConsoleColorMap), typeof(Console));

        private RichTextBox _host;
        private TextPointer _inputStart;
        private Shape _inputMarker;
        private Shape _caretMarker;

        public IConsoleModel Model
        {
            get { return (IConsoleModel)(GetValue(ModelProperty) ?? ConsoleModel.Null); }
            set { SetValue(ModelProperty, value); }
        }

        public bool DisplayDebugMarkers
        {
            get { return (bool)(GetValue(DisplayDebugMarkersProperty) ?? false); }
            set { SetValue(DisplayDebugMarkersProperty, value); }
        }

        public IConsoleColorMap ColorMap
        {
            get { return (IConsoleColorMap)(GetValue(ColorMapProperty) ?? ConsoleColorMap.Default); }
            set { SetValue(ColorMapProperty, value); }
        }

        private TextPointer InputStart
        {
            get { return _inputStart; }
            set
            {
                if (_inputStart == null || !_inputStart.Equals(value))
                {
                    _inputStart = value;
                    UpdateDebugRenderings();
                }
            }
        }

        static Console()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Console), new FrameworkPropertyMetadata(typeof(Console)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _host = (RichTextBox)GetTemplateChild("PART_Host");

            InputStart = _host.CaretPosition;
            _host.PreviewKeyDown += _host_PreviewKeyDown;
            _host.PreviewTextInput += _host_PreviewTextInput;

            if (_host.Document != null)
            {
                foreach (var block in _host.Document.Blocks)
                {
                    block.Margin = new Thickness(0);
                }
            }

            PrepDebugReferences();
        }

        private static void AdjustMarker(Shape marker, TextPointer pointer)
        {
            var rect = pointer.GetCharacterRect(LogicalDirection.Forward);
            marker.Margin = new Thickness(rect.Left, rect.Top, 0, 0);
            marker.Visibility = System.Windows.Visibility.Visible;
            marker.Fill = pointer.LogicalDirection == LogicalDirection.Forward ? Brushes.HotPink : Brushes.Yellow;
        }

        private static void ConsoleModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IConsoleModel oldModel = e.OldValue as IConsoleModel;
            IConsoleModel newModel = e.NewValue as IConsoleModel;
            IConsoleHost host = d as IConsoleHost;
            if (host != null)
            {
                if (oldModel != null)
                {
                    oldModel.Detach(host);
                }
                if (newModel != null)
                {
                    newModel.Attach(host);
                }
            }
        }

        private void _host_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // Get all the text since the input started
                var committedRange = new TextRange(_inputStart, _inputStart.DocumentEnd);
                var committedText = committedRange.Text;
                if (committedText.EndsWith(Environment.NewLine))
                {
                    committedText = committedText.Substring(0, committedText.Length - Environment.NewLine.Length);
                }
                _host.CaretPosition = _host.Document.ContentEnd;
                InputStart = _host.CaretPosition;
                Model.LineCommitted(committedText);
                e.Handled = true;
            }
            else if (e.Key == Key.Back && _host.CaretPosition.CompareTo(_inputStart) <= 0)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Delete && _host.CaretPosition.CompareTo(_inputStart) < 0)
            {
                e.Handled = true;
            }
        }

        private void _host_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_inputStart != null && _host.CaretPosition.CompareTo(_inputStart) < 0)
            {
                e.Handled = true;
            }
        }

        private Task Write(FormattedTextRun run)
        {
            var start = _host.Document.ContentStart.GetOffsetToPosition(_host.CaretPosition);
            _host.CaretPosition.InsertTextInRun(run.Text);

            var startPos = _host.Document.ContentStart.GetPositionAtOffset(start, LogicalDirection.Forward);
            var endPos = _host.Document.ContentStart.GetPositionAtOffset(start + run.Text.Length, LogicalDirection.Forward);
            var range = new TextRange(startPos, endPos);
            run.Classification.ApplyToRange(range, ColorMap);
            
            InputStart = range.End
                              .GetPositionAtOffset(0, LogicalDirection.Backward);

            _host.CaretPosition = InputStart;
            return TaskEx.FromCompleted();
        }

        async Task IConsoleHost.Write(FormattedText text)
        {
            foreach (var run in text.Runs)
            {
                await Write(run);
            }
        }

        Task IConsoleHost.InsertLineBreak()
        {
            InputStart = InputStart.InsertParagraphBreak();
            InputStart.Paragraph.Margin = new Thickness(0);
            _host.CaretPosition = InputStart;
            return TaskEx.FromCompleted();
        }

        Task IConsoleHost.Clear()
        {
            _host.Document = new FlowDocument(new Paragraph(new Run()));
            _host.CaretPosition = _host.Document.ContentStart;
            return TaskEx.FromCompleted();
        }

        #region Debug Rendering Code

        private void PrepDebugReferences()
        {
            if (DisplayDebugMarkers)
            {
                _inputMarker = GetTemplateChild("PART_Debug_InsertMarker") as Shape;
                _caretMarker = GetTemplateChild("PART_Debug_CaretMarker") as Shape;
                if (_inputMarker != null || _caretMarker != null)
                {
                    _host.TextChanged += DebugTextChanged;
                    _host.SelectionChanged += DebugSelectionChanged;
                }
            }
        }

        private void DebugSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!DisplayDebugMarkers)
            {
                _host.SelectionChanged -= DebugSelectionChanged;
            }
            else
            {
                UpdateDebugRenderings();
            }
        }

        private void DebugTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!DisplayDebugMarkers)
            {
                _host.TextChanged -= DebugTextChanged;
            }
            else
            {
                UpdateDebugRenderings();
            }
        }

        private void UpdateDebugRenderings()
        {
            if (_inputMarker != null && _inputStart != null)
            {
                AdjustMarker(_inputMarker, _inputStart);
            }
            if (_caretMarker != null && _host.CaretPosition != null)
            {
                AdjustMarker(_caretMarker, _host.CaretPosition);
            }
        }

        #endregion
    }
}
