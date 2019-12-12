using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using IronPdf;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Festispec.Models.Answers;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel: ViewModelBase
    {
        private IQuestionService _questionService;
        private IQuestionnaireService _questionnaireService;
        public ObservableCollection<Control> Charts { get; set; }
        public Festival selectedFestival { get; set; }

        private string PdfHtml = "";

        public ICommand GeneratePdfCommand { get; set; }
        public RapportPreviewViewModel(IQuestionService questionService, IQuestionnaireService questionnaireService)
        {
            _questionService = questionService;
            _questionnaireService = questionnaireService;

            GenerateReport(2);
        }


        private async void GenerateReport(int questionaireId)
        {

            var questionaire = _questionnaireService.GetQuestionnaire(questionaireId);
            var questions = await _questionnaireService.GetQuestionsFromQuestionnaire(questionaireId);
            Charts = new ObservableCollection<Control>();


            PdfHtml += String.Format("<h1>{0}</h1>", questionaire.Name);

            foreach (var question in questions)
            {
                AddQuestionToReport(question);
            }

            GeneratePdfCommand = new RelayCommand(SavePdf);
        }


        private void AddQuestionToReport(Question question)
        {

            // Answers
            var converter = new GraphSelectorFactory().GetConverter(question.GraphType);


            if (converter == null && !(question is UploadPictureQuestion || question is StringQuestion))
            {
                return;
            }

            else if (question is UploadPictureQuestion || question is StringQuestion)
            {
                // String question or image upload.
                if (question is UploadPictureQuestion)
                {
                    return;
                }
                else if(question is StringQuestion)
                {
                    // String.
                    Charts.Add(CreateLabel(question.Contents));
                    var stringAnswer = new TextBox { Height = 150, Width = 700, Margin = new Thickness(10) };
                    return;
                }
            }

            var chartValues = converter.TypeToChart(question);

            if (chartValues.Count < 1)
                return;

            var lineControl = new Control();
            if (question.GraphType == Models.GraphType.Line)
            {
                lineControl = new LineChartControl(chartValues);
                lineControl.Height = 300;
                lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }
            if (question.GraphType == Models.GraphType.Pie)
            {
                lineControl = new PieChartControl(chartValues);
                lineControl.Height = 300;
                lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }
            else if (question.GraphType == Models.GraphType.Column)
            {
                lineControl = new ColumnChartControl(chartValues);
                lineControl.Height = 300;
                lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }


            Charts.Add(CreateLabel(question.Contents));
            var textBox = new TextBox { Height = 150, Width = 700, Margin = new Thickness(10) };
            Charts.Add(lineControl);
            Charts.Add(textBox);
        }

        private void SavePdf()
        {
            foreach(var chart in Charts)            
                AddControlToPdf(chart);
            

            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var renderPath = Path.Combine(path, "report-Festispec.pdf");

            Renderer.RenderHtmlAsPdf(PdfHtml).SaveAs(renderPath);

            MessageBox.Show("Generated!");
        }

        private void AddControlToPdf(Control control)
        {
            if(control is TextBox)
            {
                var textBox = (TextBox)control;
                string richText = textBox.Text;

                PdfHtml += String.Format("<p>{0}</p>", richText);
            }
            else if(control is Label)
            {
                var label = (Label)control;
                PdfHtml += String.Format("<h2>{0}</h2>", label.Content);
            }
            else
            {
                var file = WriteToPng(control);
                PdfHtml += String.Format("<img src='{0}'>", file);
            }
        }

        public string WriteToPng(UIElement element)
        {
            var rect = new Rect(element.RenderSize);
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
                dc.DrawRectangle(new VisualBrush(element), null, rect);
            

            var bitmap = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";

            using (var file = File.OpenWrite(fileName))
                encoder.Save(file);

            return fileName;
        }


        private Label CreateLabel(string text)
        {
            Label label = new Label();

            label.Content = text;

            label.Width = Double.NaN;

            label.Height = 30;

            label.Foreground = new SolidColorBrush(Colors.White);

            label.Background = new SolidColorBrush(Colors.Black);


            return label;
        }
    }
}
