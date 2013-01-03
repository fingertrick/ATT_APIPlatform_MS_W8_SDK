// <copyright file="MmsGalleryControlPresenter .cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ATT.Controls.Utility;
using ATT.Services;
using ATT.Utility;

namespace ATT.Controls.Presenters
{
    /// <summary>
    /// Presenter for <see cref="MmsGalleryControl"/>
    /// </summary>
    public class MmsGalleryControlPresenter : PresenterBase
    {
        private readonly IMmsStorage _mmsStorage;
        private readonly string _shortCode;
       
        private ObservableCollection<InboundMms> _mmsCollection;

        /// <summary>
        /// Creates instance of <see cref="MmsGalleryControlPresenter"/>
        /// </summary>
        /// <param name="shortCode"></param>
        /// <param name="mmsStorage"></param>
        public MmsGalleryControlPresenter(string shortCode, IMmsStorage mmsStorage)
        {
            Argument.ExpectNotNullOrWhiteSpace(() => shortCode);

            _shortCode = shortCode;
            _mmsStorage = mmsStorage;           
            UpdateMmsGalleryCommand = new RelayCommand((obj) => UpdateMMS(obj));

            LoadImages(_shortCode);           
        }

        private async void LoadImages(string shortCode)
        {
            IEnumerable<InboundMms> messages = await _mmsStorage.GetMmsMessages(shortCode);
            _mmsCollection = new ObservableCollection<InboundMms>(messages);              
        }

        private async void UpdateMMS(object parameter)
        {
            try
            {
                //TODO: for testing

                var newMMS = new List<InboundMms>();
                var attachments = new List<string> {"1.jpg", "2.jpg"};
                newMMS.Add(new InboundMms("1", new PhoneNumber(_shortCode), "body", attachments));
                newMMS.Add(new InboundMms("2", new PhoneNumber(_shortCode), "body1", attachments));

                await _mmsStorage.StoreMmsMessages(newMMS, _shortCode);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
      
        /// <summary>
        /// Gets collection of MMS
        /// </summary>
        public ObservableCollection<InboundMms> MmsCollection
        {
            get 
            { 
                return _mmsCollection; 
            }           
        }

        /// <summary>
        /// Gets or sets command for update MMS collection from storage
        /// </summary>
        public RelayCommand UpdateMmsGalleryCommand { get; protected set; }

        /// <summary>
        /// Unload presenter. Release resources which was used in presenter
        /// </summary>
		public override void Unload()
		{
			
		}        
    }
}

