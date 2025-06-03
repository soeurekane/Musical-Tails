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

namespace MusicalTails
{
    public partial class InputDialog : Window
    {
        public string Answer { get; private set; }

        public InputDialog(string question)
        {
            InitializeComponent();
            QuestionText.Text = question;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Answer = AnswerTextBox.Text;
            DialogResult = true;
        }
    }
}
