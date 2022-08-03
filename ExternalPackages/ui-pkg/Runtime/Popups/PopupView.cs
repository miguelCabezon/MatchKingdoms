using System;
using UnityEngine;
using JackSParrot.Utils;
using JackSParrot.Audio;

namespace JackSParrot.UI
{
    public interface IPopupConfig
    {
        string PrefabAddress { get; }
    }

    public class PopupView : BaseView
    {
        [SerializeField]
        ClipId _onAppearSound = null;
        [SerializeField]
        ClipId _onHideSound = null;

        IPopupConfig _config = null;

        public virtual void Initialize(IPopupConfig config)
        {
            _config = config;
        }

        public override void Show(bool animated = true, Action onFinish = null)
        {
            base.Show(animated, onFinish);
            if (_config == null)
            {
                SharedServices.GetService<ICustomLogger>()?.LogError("Showing a popup not initialized");
            }

            if (_onAppearSound.IsValid())
            {
                SharedServices.GetService<AudioService>()?.PlaySfx(_onAppearSound);
            }
        }

        public override void Hide(bool animated = true, Action onFinish = null)
        {
            base.Hide(animated, onFinish);
            if (_onHideSound.IsValid())
            {
                SharedServices.GetService<AudioService>()?.PlaySfx(_onHideSound);
            }
        }

        protected override void OnHidden(Action callback)
        {
            SharedServices.GetService<UIService>().PopPopup();
            base.OnHidden(callback);
        }

        public void Close(Action callback = null)
        {
            Hide(true, callback);
        }
    }
}