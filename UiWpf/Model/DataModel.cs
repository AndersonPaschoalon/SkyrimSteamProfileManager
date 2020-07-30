using SteamProfileManager.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiWpf.Model
{
    class DataModel : INotifyPropertyChanged
    {
        public DataModel()
        {
            this.nmmInfoPath = "";
            this.nmmModPath = "";
            this.steamPath = "";
            this.appDirPath = "";
            this.docsPath = "";
            this.state = SteamProfileManager.SteamProfileManager.State.NO_PROFILE;
            this.activeProf = null;
            this.desactivatedProf = new List<SPProfile>();
        }

        #region select_game

        public string selectedGame 
        {
            get { return this._selectedGame; }
            set
            {
                if (this._selectedGame != value)
                {
                    this._selectedGame = value;
                    RaisePropertyChanged("selectedGame");
                }
            }
        }

        #endregion select_game

        #region settings 

        public string steamPath 
        {
            get { return this._steamPath; }
            set
            {
                if (this._steamPath != value)
                {
                    this._steamPath = value;
                    RaisePropertyChanged("steamPath");
                }
            }
        }
        public string appDirPath 
        {
            get { return this._appDirPath; }
            set
            {
                if (this._appDirPath != value)
                {
                    this._appDirPath = value;
                    RaisePropertyChanged("appDirPath");
                }
            }
        }
        public string docsPath 
        {
            get { return this._docsPath; }
            set
            {
                if (this._docsPath != value)
                {
                    this._docsPath = value;
                    RaisePropertyChanged("docsPath");
                }
            }
        }
        public string nmmInfoPath 
        {
            get { return this._nmmInfoPath; }
            set
            {
                if (this._nmmInfoPath != value)
                {
                    this._nmmInfoPath = value;
                    RaisePropertyChanged("nmmInfoPath");
                }
            }
        }
        public string nmmModPath 
        {
            get { return this._nmmModPath; }
            set
            {
                if (this._nmmModPath != value)
                {
                    this._nmmModPath = value;
                    RaisePropertyChanged("nmmModPath");
                }
            }
        }

        #endregion settings 



        public SteamProfileManager.SteamProfileManager.State state 
        {
            get { return this._state; }
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    RaisePropertyChanged("state");
                }
            }
        }

        public SPProfile activeProf 
        {
            get { return this._activeProf; }
            set
            {
                if (this._activeProf != value)
                {
                    this._activeProf = value;
                    RaisePropertyChanged("activeProf");
                }
            }
        }

        public List<SPProfile> desactivatedProf 
        {
            get { return this._desactivatedProf; }
            set
            {
                if (this._desactivatedProf != value)
                {
                    this._desactivatedProf = value;
                    RaisePropertyChanged("desactivatedProf");
                }
            }
        }

        // ????????
        #region checkboxes_selected
        public string activeSelected
        {
            get { return this._activeSelected; }
            set
            {
                if (this._activeSelected != value)
                {
                    this._activeSelected = value;
                    RaisePropertyChanged("activeSelected");
                }
            }
        }
        public string inactiveSelected
        {
            get { return this._inactiveSelected; }
            set
            {
                if (this._inactiveSelected != value)
                {
                    this._inactiveSelected = value;
                    RaisePropertyChanged("inactiveSelected");
                }
            }
        }

        public string desactivatedSelected
        {
            get { return this._desactivatedSelected; }
            set
            {
                if (this._desactivatedSelected != value)
                {
                    this._desactivatedSelected = value;
                    RaisePropertyChanged("desactivatedSelected");
                }
            }
        }


        #endregion checkboxes_selected

        #region btns_state

        public bool updateBtnActivateActive()
        {
            bool btnVal = false;
            if (this.state == SteamProfileManager.SteamProfileManager.State.DESACTIVATED_ONLY ||
               this.state == SteamProfileManager.SteamProfileManager.State.INACTIVE_PROFILE)
            {
                if (activeSelected == "")
                {
                    if (inactiveSelected != "" || desactivatedSelected != "")
                    {
                        btnVal = true;
                    }
                }
            }
            this.btnActivateActive = btnVal;
            return btnVal;
        }
        public bool updateBtnDesactivate()
        {
            bool btnVal = false;
            if (this.state == SteamProfileManager.SteamProfileManager.State.ACTIVE_AND_DESACTIVATED_PROFILES ||
               this.state == SteamProfileManager.SteamProfileManager.State.ACTIVE_ONLY)
            {
                if (inactiveSelected == "" && desactivatedSelected == "")
                {
                    if (activeSelected != "")
                    {
                        btnVal = true;
                    }
                }

            }
            this.btnDesactivateActive = btnVal;
            return btnVal;
        }

        public bool updateBtnSwitch()
        {
            bool btnVal = false;
            if (this.state == SteamProfileManager.SteamProfileManager.State.ACTIVE_AND_DESACTIVATED_PROFILES)
            {
                if (this.activeSelected != "" && this.desactivatedSelected != "")
                {
                    btnVal = true;
                }
            }
            this.btnSwitchActive = btnVal;
            return btnVal;
        }

        public bool btnActivateActive
        {
            get { return this._btnActivateActive; }
            private set
            {
                if (this._btnActivateActive != value)
                {
                    this._btnActivateActive = value;
                    RaisePropertyChanged("btnActivateActive");
                }
            }
        }
        public bool btnDesactivateActive
        {
            get { return this._btnDesactivateActive; }
            private set
            {
                if (this._btnDesactivateActive != value)
                {
                    this._btnDesactivateActive = value;
                    RaisePropertyChanged("btnDesactivateActive");
                }
            }
        }
        public bool btnSwitchActive
        {
            get { return this._btnSwitchActive; }
            private set
            {
                if (this._btnSwitchActive != value)
                {
                    this._btnSwitchActive = value;
                    RaisePropertyChanged("btnSwitchActive");
                }
            }
        }

        #endregion btns_state

        #region profilesChecked

        #region

        #region view_model

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private string _selectedGame;
        private string _steamPath;
        private string _appDirPath;
        private string _docsPath;
        private string _nmmInfoPath;
        private string _nmmModPath;
        private SteamProfileManager.SteamProfileManager.State _state;
        private SPProfile _activeProf;
        private List<SPProfile> _desactivatedProf;
        private bool _btnActivateActive;
        private bool _btnDesactivateActive;
        private bool _btnSwitchActive;
        private string _activeSelected = "";
        private string _inactiveSelected = "";
        private string _desactivatedSelected = "";

        #endregion view_model
    }
}
