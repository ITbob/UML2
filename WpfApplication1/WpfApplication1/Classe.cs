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
using System.Drawing;
using System.Windows.Shapes;


namespace WpfApplication1
{
    class Classe
    {
        private List<Rectangle> cadres;

        private String type;

        private RichTextBox variables_tb;
        private Label variables;

        private RichTextBox fonctions_tb;
        private Label fonctions;

        private TextBox nom_tb;
        private Label nom;

        public Label Fonctions
        {
            get { return fonctions; }
            set { fonctions = value; }
        }

        public Label Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        public RichTextBox Variables_tb
        {
            get { return variables_tb; }
            set { variables_tb = value; }
        }

        public RichTextBox Fonctions_tb
        {
            get { return fonctions_tb; }
            set { fonctions_tb = value; }
        }

        public TextBox Nom_tb
        {
            get { return nom_tb; }
            set { nom_tb = value; }
        }

        public List<Rectangle> Cadres
        {
            get { return cadres; }
            set { cadres = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public Label Nom
        {
            get { return nom; }
            set { nom = value; }
        }
    }
}
