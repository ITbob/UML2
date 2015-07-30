using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        private Button nP;
        private List<Label> LPP;
        private List<Label> LPN;
        private List<String> hidden;
        public event EventHandler<MessageEvent> message;

        public FirstWindow()
        {
            LPP = new List<Label>();
            LPN = new List<Label>();
            hidden = new List<string>();
        
            nP = new Button();
            nP.Name = "np";
            nP.Click += newProject;
            this.Closed += close;

            InitializeComponent();
        }

        private void newProject(object sender, RoutedEventArgs e)
        {
            if (message != null)
                message(this, new MessageEvent { type = "wpf", value = "mainwindow" });
        }

        private void close(object sender, EventArgs e)
        {
            Console.WriteLine("ALLO");
            if (message != null)
                message(this, new MessageEvent { type = "exit", value = "exit" });
        }

        public void click(object sender, MouseEventArgs e)
        {
            Label lab = (Label)sender;
            int numb = 0;
            Int32.TryParse(lab.Content.ToString().Substring(0, 1),out numb);

            if (message != null)
                message(this, new MessageEvent { type = "open", value = hidden[numb] });
        }

        public void change(object sender, MouseEventArgs e)
        {
            Label lab = (Label)sender;
            lab.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        public void change2(object sender, MouseEventArgs e)
        {
            Label lab = (Label)sender;
            lab.Foreground = new SolidColorBrush(Color.FromRgb(122, 189, 255));
        }

        public void updateLast(List<String> value)
        {
            hidden = value;

            for (int i = LPP.Count; i < value.Count; i++)
            {
                LPP.Add(new Label
                {
                    Content = "..." + value[i].Substring(value[i].Length-15,15),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                });

                LPN.Add(new Label
                {
                    Content = i + ". " + System.IO.Path.GetFileName(value[i]),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center, 
                });

                int last = LPP.Count - 1;

                LPN[last].FontWeight = FontWeights.Bold;
                LPN[last].Foreground = new SolidColorBrush(Color.FromRgb(122, 189, 255)); //NOT BAD

                LPP[last].Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                LPP[last].Arrange(new Rect(lp.DesiredSize));
                LPP[last].FontWeight = FontWeights.Bold;
                LPP[last].Foreground = new SolidColorBrush(Color.FromRgb(43, 43, 43)); //NOT BAD

                LPN[last].MouseDown += click;
                LPN[last].MouseMove += change;
                LPN[last].MouseLeave += change2;

                back3.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(LPP[last].ActualHeight),
                });

                back3.Children.Add(LPP[last]);
                back3.Children.Add(LPN[last]);
                Grid.SetRow(LPN[last], last);
                Grid.SetRow(LPP[last], last);
                Grid.SetColumn(LPN[last], 0);
                Grid.SetColumn(LPP[last], 1);


                Console.WriteLine("v : " + value[i]);
                Console.WriteLine(lp.ActualHeight);
            }
        }
    }
}
