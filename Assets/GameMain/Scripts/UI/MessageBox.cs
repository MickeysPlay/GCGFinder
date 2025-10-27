using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
using System;
using TMPro;

namespace GameMain.UI
{
    /// <summary>
    /// MessageBox Type
    /// </summary>
    public enum MessageBoxType
    {
        Info,
        Warning,
        Error,
        Confirm
    }

    /// <summary>
    /// MessageBox Data
    /// </summary>
    public class MessageBoxData
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageBoxType Type { get; set; } = MessageBoxType.Info;
        public Action OnConfirm { get; set; }
        public Action OnCancel { get; set; }
        public string ConfirmButtonText { get; set; } = "OK";
        public string CancelButtonText { get; set; } = "Cancel";
    }

    /// <summary>
    /// Message Box Panel
    /// Displays info, warning, error, or confirmation messages
    /// </summary>
    public class MessageBox : UIFormBase
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundPanel;

        [Header("Icons")]
        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite warningIcon;
        [SerializeField] private Sprite errorIcon;

        [Header("Colors")]
        [SerializeField] private Color infoColor = new Color(0.17f, 0.32f, 0.70f); // Gundam Blue #2C52B3
        [SerializeField] private Color warningColor = new Color(1f, 0.97f, 0.40f); // Gundam Yellow #FFF867
        [SerializeField] private Color errorColor = new Color(0.98f, 0.18f, 0.22f); // Gundam Red #FB2F38

        private MessageBoxData currentData;

        protected override void OnInit()
        {
            base.OnInit();

            // Bind button events
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(OnConfirmClicked);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelClicked);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            currentData = userData as MessageBoxData;
            if (currentData == null)
            {
                Debug.LogError("[MessageBox] MessageBoxData is null!");
                Close();
                return;
            }

            UpdateUI();
        }

        protected override void OnClose()
        {
            base.OnClose();
            currentData = null;
        }

        /// <summary>
        /// Update UI based on MessageBoxData
        /// </summary>
        private void UpdateUI()
        {
            if (currentData == null) return;

            // Set title
            if (titleText != null)
            {
                titleText.text = string.IsNullOrEmpty(currentData.Title)
                    ? GetDefaultTitle(currentData.Type)
                    : currentData.Title;
            }

            // Set message
            if (messageText != null)
            {
                messageText.text = currentData.Message ?? string.Empty;
            }

            // Setup by type
            SetupByType(currentData.Type);

            // Set button text
            SetupButtonText();
        }

        /// <summary>
        /// Get default title based on type
        /// </summary>
        private string GetDefaultTitle(MessageBoxType type)
        {
            return type switch
            {
                MessageBoxType.Info => "Information",
                MessageBoxType.Warning => "Warning",
                MessageBoxType.Error => "Error",
                MessageBoxType.Confirm => "Confirmation",
                _ => "Message"
            };
        }

        /// <summary>
        /// Setup UI based on message type
        /// </summary>
        private void SetupByType(MessageBoxType type)
        {
            // Set icon
            if (iconImage != null)
            {
                Sprite icon = type switch
                {
                    MessageBoxType.Info => infoIcon,
                    MessageBoxType.Warning => warningIcon,
                    MessageBoxType.Error => errorIcon,
                    MessageBoxType.Confirm => infoIcon,
                    _ => null
                };

                iconImage.sprite = icon;
                iconImage.gameObject.SetActive(icon != null);

                // Set icon color
                if (icon != null)
                {
                    iconImage.color = GetColorByType(type);
                }
            }

            // Set background color accent
            if (backgroundPanel != null && titleText != null)
            {
                titleText.color = GetColorByType(type);
            }

            // Setup buttons
            bool showCancel = type == MessageBoxType.Confirm;
            if (cancelButton != null)
            {
                cancelButton.gameObject.SetActive(showCancel);
            }
        }

        /// <summary>
        /// Get color based on message type
        /// </summary>
        private Color GetColorByType(MessageBoxType type)
        {
            return type switch
            {
                MessageBoxType.Info => infoColor,
                MessageBoxType.Warning => warningColor,
                MessageBoxType.Error => errorColor,
                MessageBoxType.Confirm => infoColor,
                _ => Color.white
            };
        }

        /// <summary>
        /// Setup button text
        /// </summary>
        private void SetupButtonText()
        {
            if (currentData == null) return;

            // Confirm button
            if (confirmButton != null)
            {
                var btnText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = currentData.ConfirmButtonText;
                }
            }

            // Cancel button
            if (cancelButton != null && cancelButton.gameObject.activeSelf)
            {
                var btnText = cancelButton.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = currentData.CancelButtonText;
                }
            }
        }

        /// <summary>
        /// Handle confirm button click
        /// </summary>
        private void OnConfirmClicked()
        {
            currentData?.OnConfirm?.Invoke();
            Close();
        }

        /// <summary>
        /// Handle cancel button click
        /// </summary>
        private void OnCancelClicked()
        {
            currentData?.OnCancel?.Invoke();
            Close();
        }

        /// <summary>
        /// Helper method to show info message
        /// </summary>
        public static void ShowInfo(string message, string title = null, Action onConfirm = null)
        {
            var uiManager = GameFramework.GameFrameworkEntry.GetModule<IUIManager>();
            uiManager?.OpenUIForm<MessageBox>(UIGroup.Tips, new MessageBoxData
            {
                Type = MessageBoxType.Info,
                Title = title,
                Message = message,
                OnConfirm = onConfirm
            });
        }

        /// <summary>
        /// Helper method to show error message
        /// </summary>
        public static void ShowError(string message, string title = null, Action onConfirm = null)
        {
            var uiManager = GameFramework.GameFrameworkEntry.GetModule<IUIManager>();
            uiManager?.OpenUIForm<MessageBox>(UIGroup.Tips, new MessageBoxData
            {
                Type = MessageBoxType.Error,
                Title = title,
                Message = message,
                OnConfirm = onConfirm
            });
        }

        /// <summary>
        /// Helper method to show warning message
        /// </summary>
        public static void ShowWarning(string message, string title = null, Action onConfirm = null)
        {
            var uiManager = GameFramework.GameFrameworkEntry.GetModule<IUIManager>();
            uiManager?.OpenUIForm<MessageBox>(UIGroup.Tips, new MessageBoxData
            {
                Type = MessageBoxType.Warning,
                Title = title,
                Message = message,
                OnConfirm = onConfirm
            });
        }

        /// <summary>
        /// Helper method to show confirmation dialog
        /// </summary>
        public static void ShowConfirm(string message, Action onConfirm, Action onCancel = null, string title = null)
        {
            var uiManager = GameFramework.GameFrameworkEntry.GetModule<IUIManager>();
            uiManager?.OpenUIForm<MessageBox>(UIGroup.Tips, new MessageBoxData
            {
                Type = MessageBoxType.Confirm,
                Title = title,
                Message = message,
                OnConfirm = onConfirm,
                OnCancel = onCancel
            });
        }
    }
}
