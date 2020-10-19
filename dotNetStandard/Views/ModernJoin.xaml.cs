using Atomus.Page.Join.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atomus.Page.Join
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModernJoin : CarouselPage, ICore
    {
        #region INIT
        public ModernJoin()
        {
            this.BindingContext = new ModernJoinViewModel(this);

            InitializeComponent();

            //this.BackgroundColor = ((string)Config.Client.GetAttribute("BackgroundColor")).ToColor();
        }
        #endregion

        #region EVENT
        protected override void OnAppearing() { }

        protected override bool OnBackButtonPressed()
        {
            if (this.CurrentPage == this.Children[1])
            {
                this.CurrentPage = this.Children[0];
                return true;
            }

            base.OnBackButtonPressed();
            return false;
        }
        #endregion

        #region ETC
        #endregion
    }
}