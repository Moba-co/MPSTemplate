using Microsoft.Extensions.DependencyInjection;
using NToastNotify;

namespace MPS.IOC.Configurations.ToastNotify
{
    public static class NToastNotificationRegistry
    {
        public static IMvcCoreBuilder AddToastWithOptions(this IMvcCoreBuilder builder)
        {
            builder.AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PreventDuplicates = true,
                NewestOnTop = true
            }, new NToastNotifyOption
            {
                DefaultSuccessTitle = "موفقیت",
                DefaultAlertTitle = "هشدار",
                DefaultWarningTitle = "اخطار",
                DefaultErrorTitle = "خطا"

            });
            return builder;
        }

        public static IMvcCoreBuilder AddNotifyWithOptions(this IMvcCoreBuilder builder)
        {
            builder.AddNToastNotifyNoty(new NotyOptions()
            {
                ProgressBar = true
            }, new NToastNotifyOption
            {
                DefaultSuccessTitle = "موفقیت",
                DefaultAlertTitle = "هشدار",
                DefaultWarningTitle = "اخطار",
                DefaultErrorTitle = "خطا"

            });
            return builder;
        }
    }
}