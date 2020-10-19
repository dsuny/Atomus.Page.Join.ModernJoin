using Atomus.Page.Join.Controllers;
using Atomus.Security;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Atomus.Page.Join.ViewModel
{
    public class ModernJoinViewModel : MVVM.ViewModel
    {
        #region Declare
        private ICore core;

        private bool userAgreementIsToggled;
        private string userAgreement;
        private bool personalInformationCollectionAgreementToggled;
        private string personalInformationCollectionAgreement;

        private string email;
        private string reEmail;
        private string accessNumber;
        private string reAccessNumber;
        private string nickname;
        private decimal referral;

        private bool activityIndicator;
        private bool isEnabledControl;
        #endregion

        #region Property
        public ICore Core
        {
            get
            {
                return this.core;
            }
            set
            {
                this.core = value;
            }
        }
        public string AppName
        {
            get
            {
                return Config.Client.GetAttribute("App.Name").ToString();
            }
            set
            {
                NotifyPropertyChanged();
            }
        }

        public bool UserAgreementIsToggled
        {
            get
            {
                return this.userAgreementIsToggled;
            }
            set
            {
                if (this.userAgreementIsToggled != value)
                {
                    this.userAgreementIsToggled = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("AgreementIsToggled");
                }
            }
        }
        public string UserAgreement
        {
            get
            {
                return this.userAgreement;
            }
            set
            {
                if (this.userAgreement != value)
                {
                    this.userAgreement = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool PersonalInformationCollectionAgreementToggled
        {
            get
            {
                return this.personalInformationCollectionAgreementToggled;
            }
            set
            {
                if (this.personalInformationCollectionAgreementToggled != value)
                {
                    this.personalInformationCollectionAgreementToggled = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("AgreementIsToggled");
                }
            }
        }
        public string PersonalInformationCollectionAgreement
        {
            get
            {
                return this.personalInformationCollectionAgreement;
            }
            set
            {
                if (this.personalInformationCollectionAgreement != value)
                {
                    this.personalInformationCollectionAgreement = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AgreementIsToggled
        {
            get
            {
                return this.userAgreementIsToggled && this.personalInformationCollectionAgreementToggled;
            }
            set
            {
                if ((this.userAgreementIsToggled && this.personalInformationCollectionAgreementToggled) != value)
                {
                    this.UserAgreementIsToggled = value;
                    this.PersonalInformationCollectionAgreementToggled = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ReEmail
        {
            get
            {
                return this.reEmail;
            }
            set
            {
                if (this.reEmail != value)
                {
                    this.reEmail = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string AccessNumber
        {
            get
            {
                return this.accessNumber;
            }
            set
            {
                if (this.accessNumber != value)
                {
                    this.accessNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ReAccessNumber
        {
            get
            {
                return this.reAccessNumber;
            }
            set
            {
                if (this.reAccessNumber != value)
                {
                    this.reAccessNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Nickname
        {
            get
            {
                return this.nickname;
            }
            set
            {
                if (this.nickname != value)
                {
                    this.nickname = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Referral
        {
            get
            {
                return this.referral.ToString("00000");
            }
            set
            {
                decimal tmp;

                if (value.ToTryDecimal(out tmp) && this.referral != tmp)
                {
                    this.referral = tmp;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ActivityIndicator
        {
            get
            {
                return this.activityIndicator;
            }
            set
            {
                if (this.activityIndicator != value)
                {
                    this.activityIndicator = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsEnabledControl
        {
            get
            {
                return this.isEnabledControl;
            }
            set
            {
                if (this.isEnabledControl != value)
                {
                    this.isEnabledControl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand AgreementNextCommand { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand BackCommand { get; set; }
        #endregion

        #region INIT
        public ModernJoinViewModel() { }
        public ModernJoinViewModel(ICore core) : this()
        {
            this.Core = core;

            this.userAgreementIsToggled = false;
            this.userAgreement = this.core.GetAttribute("UserAgreement");
            this.personalInformationCollectionAgreementToggled = false;
            this.personalInformationCollectionAgreement = this.core.GetAttribute("PersonalInformationCollectionAgreement");

            this.email = "";
            this.reEmail = "";
            this.accessNumber = "";
            this.reAccessNumber = "";
            this.nickname = "";

            this.activityIndicator = false;
            this.isEnabledControl = true;

            this.AgreementNextCommand = new Command(() => this.AgreementNextProcess()
                                            , () => { return !this.ActivityIndicator; });

            this.JoinCommand = new Command(async () => await this.JoinProcess()
                                            , () => { return !this.ActivityIndicator; });

            this.BackCommand = new Command(async () => await this.BackProcess()
                                            , () => { return !this.ActivityIndicator; });
        }
        #endregion

        #region IO
        internal void AgreementNextProcess()
        {
            this.IsEnabledControl = false;
            this.ActivityIndicator = true;
            (this.AgreementNextCommand as Command).ChangeCanExecute();

            ((CarouselPage)this.core).CurrentPage = ((CarouselPage)this.core).Children[1];

            this.ActivityIndicator = false;
            this.IsEnabledControl = true;
            (this.AgreementNextCommand as Command).ChangeCanExecute();
        }
        private async Task JoinProcess()
        {
            Service.IResponse result;
            ISecureHashAlgorithm secureHashAlgorithm;

            try
            {
                this.IsEnabledControl = false;
                this.ActivityIndicator = true;
                (this.JoinCommand as Command).ChangeCanExecute();

                if (!this.UserAgreementIsToggled)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "이용약관 동의를 해야 진행 가능 합니다.", "OK");
                    return;
                }

                if (!this.PersonalInformationCollectionAgreementToggled)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "개인정보 수집 및 이용 동의를 해야 진행 가능 합니다.", "OK");
                    return;
                }

                if (this.Email.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "이메일을 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                if (this.AccessNumber.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "비밀번호를 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                if (this.AccessNumber != this.ReAccessNumber)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "비밀번호가 일치하지 않습니다.", "OK");
                    return;
                }

                secureHashAlgorithm = (ISecureHashAlgorithm)this.core.CreateInstance("SecureHashAlgorithm");

                if (this.Email != this.ReEmail)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "이메일이 일치하지 않습니다.", "OK");
                    return;
                }

                if (this.Nickname.IsNullOrEmpty())
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "닉네임을 입력해 주시기 바랍니다.", "OK");
                    return;
                }

                decimal tmp;

                if (!Referral.ToTryDecimal(out tmp))
                    tmp = -1;

                result = await this.core.SaveAsync(this.Email, secureHashAlgorithm.ComputeHashToBase64String(this.AccessNumber), this.Nickname, tmp);

                if (result.Status == Service.Status.OK)
                {
                    await Application.Current.MainPage.DisplayAlert("Information", "가입이 완료 되었습니다. 가입하신 이메일로 인증 메일이 발송 되었습니다. 이메일 인증을 진행해 주시기 바랍니다.", "OK");
                    await this.BackProcess();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", ex.Message, "OK");
            }
            finally
            {
                this.ActivityIndicator = false;
                this.IsEnabledControl = true;
                (this.JoinCommand as Command).ChangeCanExecute();
            }
        }

        internal async Task BackProcess()
        {
            this.IsEnabledControl = false;
            this.ActivityIndicator = true;
            (this.BackCommand as Command).ChangeCanExecute();

            await ((NavigationPage)Application.Current.MainPage).PopAsync();

            this.ActivityIndicator = false;
            this.IsEnabledControl = true;
            (this.BackCommand as Command).ChangeCanExecute();
        }
        #endregion

        #region ETC
        #endregion
    }
}