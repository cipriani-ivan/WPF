using System.Windows;

namespace InterviewAssessment.View
{
    /// <summary>
    /// Interaction logic for NamePrompt.xaml
    /// </summary>
    public partial class AddEntityDialog : Window
    {
        public string EntityName { get; private set; }

        public AddEntityDialog()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EntityName = Name.Text;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}