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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int LABEL_SPACE = 5;
        private const int SMALL_SPACE = 4;
        private List<Classe> classes;
        private List<Liaison> liaisons;
        private int selectedClass = -1;
        private int selectedFrame = -1;
        private Point premier = new Point(0, 0), deuxieme = new Point(0, 0);
        public event EventHandler<MessageEvent> message;

        public MainWindow()
        {
            this.Closed += close2;
            InitializeComponent();
        }

        public string getCursorX()
        {
            return Mouse.GetPosition(this).X.ToString();
        }

        public string getCursorY()
        {
            return Mouse.GetPosition(this).Y.ToString();
        }

        public void createNewConcept(double x, double y, String type, String name, String attributs, String methods)
        {
            if (classes == null)
                classes = new List<Classe>();

            Label nam, var, fun;
            var = new Label
            {
                Content = attributs == "" ? "attributs" : attributs,
                FontSize = 16,
                Margin = new Thickness(x, y, 0, 0),
            };


            fun = new Label
            {
                Content = methods == "" ? "methodes" : methods,
                FontSize = 16,
                Margin = new Thickness(x, y, 0, 0)
            };

            nam = new Label
            {
                Content = name == "" ? "classe : " + classes.Count : name,
                FontSize = 16,
                Margin = new Thickness(x, y, 0, 0)
            };

            nam.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            nam.Arrange(new Rect(nam.DesiredSize));

            double myWidth = nam.ActualWidth;
            double myHeight = nam.ActualHeight + LABEL_SPACE;

            Classe clas = new Classe();
            double i = 0;

            if (type.Contains("classe"))
                i = 0.5;
            else
                i = 4;

            clas.Cadres = new List<Rectangle>();
            clas.Cadres.Add(generateMyRect(myWidth + LABEL_SPACE*2, myHeight, x - LABEL_SPACE, y, i));
            clas.Cadres.Add(generateMyRect(myWidth + LABEL_SPACE * 2, myHeight, x - LABEL_SPACE, y + myHeight + 4, i));
            clas.Cadres.Add(generateMyRect(myWidth + LABEL_SPACE * 2, myHeight, x - LABEL_SPACE, y + myHeight * 2 + 8, i));
            clas.Cadres.Add(generateMyRect(LABEL_SPACE * 4, LABEL_SPACE * 4, x - LABEL_SPACE, y - LABEL_SPACE * 4, 0.5));
            clas.Cadres.Add(generateMyRect(LABEL_SPACE * 4, LABEL_SPACE * 4, x + LABEL_SPACE*3, y - LABEL_SPACE * 4, 0.5));
            //clas.Cadres.Add(generateMyRect(myWidth + LABEL_SPACE * 2, myHeight, x - LABEL_SPACE, y, i));

            clas.Cadres[3].Fill = new SolidColorBrush(Colors.Red);
            clas.Cadres[4].Fill = new SolidColorBrush(Colors.Blue);

            clas.Nom = nam;
            var.Margin = clas.Cadres[1].Margin;
            clas.Variables = var;
            fun.Margin = clas.Cadres[2].Margin;
            clas.Fonctions = fun;
            
            clas.Type = type;

            clas.Nom_tb = generateTextBox(myWidth + LABEL_SPACE * 2
                                        , myHeight
                                        , clas.Cadres[0].Margin
                                        , clas.Nom.Content.ToString());

            clas.Variables_tb = generateRichTextBox(myWidth + LABEL_SPACE * 2
                                                    , myHeight
                                                    , clas.Cadres[1].Margin);

            clas.Fonctions_tb = generateRichTextBox(myWidth + LABEL_SPACE * 2
                                                    , myHeight
                                                    , clas.Cadres[2].Margin);

            classes.Add(clas);

            back.Children.Add(clas.Cadres[0]);
            back.Children.Add(clas.Cadres[1]);
            back.Children.Add(clas.Cadres[2]);
            back.Children.Add(clas.Cadres[3]);
            back.Children.Add(clas.Cadres[4]);
            back.Children.Add(clas.Nom);
            back.Children.Add(clas.Nom_tb);
            back.Children.Add(clas.Variables);
            back.Children.Add(clas.Variables_tb);
            back.Children.Add(clas.Fonctions);
            back.Children.Add(clas.Fonctions_tb);
        }

        public Rectangle generateMyRect(double _width,double _height,double _x, double _y, double i)
        {
            return new Rectangle
            {
                Width = _width,
                Height = _height,

                Margin = new Thickness(_x, _y, 0, 0),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { i },
                Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Fill = new SolidColorBrush(Colors.White)
            };
        }

        public TextBox generateTextBox(Double myWidth, double myHeight, Thickness myMargin, String myContent)
        {
            return new TextBox
            {
                Visibility = System.Windows.Visibility.Hidden,
                Width = myWidth,
                Height = myHeight,
                Margin = myMargin,
                Text = myContent,
                
            };
        }

        public RichTextBox generateRichTextBox(Double myWidth, double myHeight, Thickness myMargin)
        {
            return new RichTextBox
            {
                Visibility = System.Windows.Visibility.Hidden,
                Width = myWidth,
                Height = myHeight,
                Margin = myMargin,
            };
        }

        public void movingItem(object sender, MouseEventArgs e)
        {
            if (selectedFrame == 3)
            {
                if (selectedClass != -1)
                {
                    double x = Mouse.GetPosition(this).X;
                    double y = Mouse.GetPosition(this).Y;
                    double myHeight = 0;

                    classes[selectedClass].Nom.Margin = new Thickness(x - myList.Width, y, 0, 0);
                    classes[selectedClass].Cadres[3].Margin = new Thickness(x - (myList.Width + LABEL_SPACE), y - LABEL_SPACE * 4, 0, 0);
                    classes[selectedClass].Cadres[4].Margin = new Thickness(x - myList.Width + LABEL_SPACE * 3, y - LABEL_SPACE * 4, 0, 0);

                    int i = 0;
                    for (i = 0; i < 3; i++)
                    {
                        classes[selectedClass].Cadres[i].Stroke = new SolidColorBrush(Color.FromRgb(200, 46, 51));
                        classes[selectedClass].Cadres[i].Margin = new Thickness(x - (myList.Width + LABEL_SPACE), y + myHeight, 0, 0);

                        myHeight += classes[selectedClass].Cadres[i].ActualHeight + LABEL_SPACE;
                    }

                    classes[selectedClass].Nom_tb.Margin = classes[selectedClass].Cadres[0].Margin;
                    classes[selectedClass].Variables_tb.Margin = classes[selectedClass].Cadres[1].Margin;
                    classes[selectedClass].Fonctions_tb.Margin = classes[selectedClass].Cadres[2].Margin;

                    classes[selectedClass].Variables.Margin = classes[selectedClass].Cadres[1].Margin;
                    classes[selectedClass].Fonctions.Margin = classes[selectedClass].Cadres[2].Margin;
                }
            }
        }

        public void updateVariables()
        {
            // WE NEED TO REMOVE AND TO RECREATE IN ORDER TO REFRESH THE LABEL DIDN'T FIND ANOTHER WAY.
            back.Children.Remove(classes[selectedClass].Variables);
            classes[selectedClass].Variables = null;
            classes[selectedClass].Variables = new Label
            {
                Margin = classes[selectedClass].Variables_tb.Margin
            };
            
            classes[selectedClass].Variables.Content = new TextRange(classes[selectedClass].Variables_tb.Document.ContentStart, classes[selectedClass].Variables_tb.Document.ContentEnd).Text;
            classes[selectedClass].Variables.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            classes[selectedClass].Variables.Arrange(new Rect(classes[selectedClass].Variables.DesiredSize));

            back.Children.Add(classes[selectedClass].Variables);
        }

        public void updateFonctions()
        {
            // WE NEED TO REMOVE AND TO RECREATE IN ORDER TO REFRESH THE LABEL DIDN'T FIND ANOTHER WAY.
            back.Children.Remove(classes[selectedClass].Fonctions);
            classes[selectedClass].Fonctions = null;
            classes[selectedClass].Fonctions = new Label
            {
                Margin = classes[selectedClass].Fonctions_tb.Margin
            };

            classes[selectedClass].Fonctions.Content = new TextRange(classes[selectedClass].Fonctions_tb.Document.ContentStart, classes[selectedClass].Fonctions_tb.Document.ContentEnd).Text;
            classes[selectedClass].Fonctions.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            classes[selectedClass].Fonctions.Arrange(new Rect(classes[selectedClass].Fonctions.DesiredSize));

            back.Children.Add(classes[selectedClass].Fonctions);
        }

        public void adaptWidth(double width)
        {
            if (width >= classes[selectedClass].Nom.ActualWidth
                    && width >= classes[selectedClass].Variables.ActualWidth
                        && width >= classes[selectedClass].Fonctions.ActualWidth)
            {
                for (int i = 0; i < classes[selectedClass].Cadres.Count - 1; i++)
                {
                    classes[selectedClass].Cadres[i].Width = width;
                }
                // RICHTEXTBOX DOES'NT ADAPT ITS SIZE FROM ITS CONTENT
                classes[selectedClass].Nom_tb.Width = width;
                classes[selectedClass].Variables_tb.Width = width;
                classes[selectedClass].Fonctions_tb.Width = width;
            }
        }

        public void changeLabel()
        {
            if(selectedFrame == 0){
                classes[selectedClass].Nom.Content = classes[selectedClass].Nom_tb.Text;
                adaptWidth(classes[selectedClass].Nom.ActualWidth);
              }else if(selectedFrame == 1)
              {
                updateVariables();

                double new_height = classes[selectedClass].Variables.ActualHeight - SMALL_SPACE * 2;

                adaptWidth(classes[selectedClass].Variables.ActualWidth);

                if (new_height > LABEL_SPACE)
                {
                    classes[selectedClass].Cadres[selectedFrame].Height = new_height;
                    classes[selectedClass].Variables_tb.Height = new_height;

                    Thickness marg = classes[selectedClass].Cadres[2].Margin;
                    marg.Top = classes[selectedClass].Cadres[1].Margin.Top + new_height + SMALL_SPACE;
                    classes[selectedClass].Cadres[2].Margin = marg;
                    classes[selectedClass].Fonctions.Margin = marg;
                    classes[selectedClass].Fonctions_tb.Margin = marg;
                }
            }
            else if (selectedFrame == 2) {
                updateFonctions();
                adaptWidth(classes[selectedClass].Fonctions.ActualWidth);
            }
            textBoxVisible(false);
        }

        public void initPoints()
        {
            premier.X = 0;
            premier.Y = 0;
            deuxieme.X = 0;
            deuxieme.Y = 0;
        }

        public void createLine()
        {
            if (premier.X == 0 && premier.Y == 0)
            {   
                premier = new Point(Mouse.GetPosition(this).X - myList.Width, Mouse.GetPosition(this).Y - 20);
            }
            else if (deuxieme.X == 0 && deuxieme.Y == 0)
            {
                deuxieme = new Point(Mouse.GetPosition(this).X - myList.Width, Mouse.GetPosition(this).Y - 20);
                if (liaisons == null)
                    liaisons = new List<Liaison>();
                liaisons.Add(new Liaison(premier, deuxieme));
                back.Children.Add(liaisons[liaisons.Count - 1].Line);
                initPoints();
            }
        }

        public void click(object sender, MouseEventArgs e)
        {

            double x = Mouse.GetPosition(this).X - myList.Width;
            double y = Mouse.GetPosition(this).Y - 20;

            myLab.Content = "X=" + getCursorX() + " Y:" + getCursorY();

            // ELEMENT SELECTED, DESELECT
            if (selectedClass  != -1){
                for (int l = 0; l < 4; l++ )
                    classes[selectedClass].Cadres[l].Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                changeLabel();
                selectedClass = -1;
                selectedFrame = -1;
            }
            // ELEMENT UNSELECTED, CHECK IF SELECT
            else
            {
                isRectangleselected(x,y);
                if (selectedClass  == -1){
                    if (myList.SelectedValue == null)
                        return;

                    Console.WriteLine((myList.SelectedItem.ToString()));

                    if (myList.SelectedItem.ToString().Contains("liaison"))
                    {
                        createLine();
                    }
                    else
                    {
                        if (premier.X != 0 && premier.Y != 0)
                            initPoints();
                        
                        createNewConcept(x, y, myList.SelectedItem.ToString(),"","","");
                    }
                }
                else
                {
                    if (selectedFrame == 0 || selectedFrame == 1 || selectedFrame == 2)
                        textBoxVisible(true);
                    else if (selectedFrame == 3)
                        back.MouseMove += movingItem;
                    else if (selectedFrame == 4)
                    {
                        removeConcept(selectedClass);
                        classes.Remove(classes[selectedClass]);
                        selectedClass = -1;
                    }

                }
            }

        }

        public void removeConcept(int l)
        {
            for (int i = 0; i < classes[l].Cadres.Count; i++)
                back.Children.Remove(classes[l].Cadres[i]);

            back.Children.Remove(classes[l].Nom);
            back.Children.Remove(classes[l].Nom_tb);
            back.Children.Remove(classes[l].Fonctions);
            back.Children.Remove(classes[l].Fonctions_tb);
            back.Children.Remove(classes[l].Variables);
            back.Children.Remove(classes[l].Variables_tb);
        }

        public void refreshBack()
        {
            foreach (UIElement child in back.Children)
            {
                if (child == null)
                {
                    back.Children.Remove(child);
                }
            }
        }

        public void textBoxVisible(Boolean visible)
        {
            if (visible)
            {
                if (selectedFrame == 0)
                {
                    classes[selectedClass].Nom_tb.Visibility = System.Windows.Visibility.Visible;
                    classes[selectedClass].Nom_tb.Focus();
                }
                else if (selectedFrame == 1)
                {
                    classes[selectedClass].Variables.Visibility = System.Windows.Visibility.Hidden;
                    classes[selectedClass].Variables_tb.Visibility = System.Windows.Visibility.Visible;
                    classes[selectedClass].Variables_tb.Focus();
                }
                else if (selectedFrame == 2)
                {
                    classes[selectedClass].Fonctions.Visibility = System.Windows.Visibility.Hidden;
                    classes[selectedClass].Fonctions_tb.Visibility = System.Windows.Visibility.Visible;
                    classes[selectedClass].Fonctions_tb.Focus();
                }
            }
            else
            {
                classes[selectedClass].Nom_tb.Visibility = System.Windows.Visibility.Hidden;
                classes[selectedClass].Variables_tb.Visibility = System.Windows.Visibility.Hidden;
                classes[selectedClass].Fonctions_tb.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void createXmlFile(String path)
        {

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            if (classes != null)
            {
                XmlNode classesNode = doc.CreateElement("classes");
                doc.AppendChild(classesNode);

                foreach (Classe clas in classes)
                {
                    XmlNode classNode = doc.CreateElement("class");
                    XmlAttribute className = doc.CreateAttribute("name");
                    className.Value = clas.Nom.Content.ToString();

                    XmlAttribute classX = doc.CreateAttribute("x");
                    classX.Value = clas.Cadres[0].Margin.Left.ToString();

                    XmlAttribute classY = doc.CreateAttribute("y");
                    classY.Value = clas.Cadres[0].Margin.Top.ToString();

                    classNode.Attributes.Append(className);
                    classNode.Attributes.Append(classX);
                    classNode.Attributes.Append(classY);
                    classesNode.AppendChild(classNode);
                }
            }
            doc.Save(path);
            //path
            message(this, new MessageEvent { type = "database_add", value = path });

        }

        public void openFile(String path)
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    //if (reader.IsStartElement())
                    //{
                        //if (reader.IsStartElement()) {
                            if (reader.IsStartElement())
                            {
                                Console.WriteLine("You are : " + reader.Name.ToString());
                                //return only when you have START tag
                                switch (reader.LocalName.ToString())
                                {
                                    case "class":
                                        int x = 0, y = 0;
                                        Int32.TryParse(reader.GetAttribute(1).ToString()
                                            ,out x);
                                        Int32.TryParse(reader.GetAttribute(2).ToString()
                                            ,out y);

                                        createNewConcept(x, y, "classe", reader.GetAttribute(0).ToString(), "", "");
                                        break;
                                }
                          //  }
                        //}
                    }
                    Console.WriteLine("");
                }
            }
        }

        public void isRectangleselected(double x, double y)
        {
            if (classes == null)
            {
                selectedClass  = -1;
                selectedFrame = -1;
                return;
            }

            int i = 0, l = 0;
            Rectangle fr;
            foreach (Classe clas in classes){
                for (l = 0; l < 5; l++)
                {
                    fr = clas.Cadres[l];
                    if (x > clas.Cadres[l].Margin.Left && x < clas.Cadres[l].Margin.Left + clas.Cadres[l].Width)
                    {
                        if (y > clas.Cadres[l].Margin.Top &&
                        y < clas.Cadres[l].Margin.Top + clas.Cadres[l].Height)
                        {
                            selectedClass = i;
                            selectedFrame = l;
                            return;
                        }
                    }
                }

                i++;
            }
            selectedClass  = -1;
            selectedFrame = -1;
        }

        //      *********
        //      FILE MENU
        //      *********

        public void save(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dial = new Microsoft.Win32.SaveFileDialog();
            dial.Filter = "Text files (*.xml)|*.xml";

            dial.ShowDialog();

            Console.WriteLine(dial.FileName);
            createXmlFile(dial.FileName);
        }

        public void close(object sender, EventArgs e)
        {
            if (message != null)
                message(this, new MessageEvent { type="wpf", value = "firstwindow" });
        }

        private void close2(object sender, EventArgs e)
        {
            Console.WriteLine("ALLO");
            if (message != null)
                message(this, new MessageEvent { type = "exit", value = "exit" });
        }

    }

}
