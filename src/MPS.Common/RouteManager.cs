namespace MPS.Common
{
    public static class RouteManager
    {
        #region
        public static string Contranct => "/قرارداد";
        public static string ContactUs => "/ارتباط-با-ما";
        public static string AboutUs => "/درباره-ما";

        #endregion

        #region product
        public static string AddNewProductPersion => "/اضافه-کردن-محصول";
        public static string AddNewProduct => "/AddNewProduct";
        public static string AddNewProduct2 => "/Prodcts/AddNewProduct";
        public static string AllProductsPersian => "/محصولات";
        public static string AllProducts => "/Products";
        public static string ProductDetails => AllProducts + "/ProductDetails";

        public static string AddedProduct => "/محصول-اضافه-شد";
        public static string AcceptRequest => AllProducts + "/AcceptProductRequest";
        public static string PayRequestPayment => "/Products/PayRequestPayment";
        #endregion

        #region auth
        private static string Auth => "/Auth";
        public static string LogOut => Auth + "/LogOut";
        public static string Login => Auth + "/Login";
        public static string Register => Auth + "/Register";
        public static string ChangeRegisterPhoneNumber => Auth + "/ChangePhone";
        public static string ForgotPassWord => Auth + "/ForgotPassword";
        #endregion

        #region profile
        public static string Profile => "/Profile";
        public static string EditProfile => Profile + "/EditUser";
        public static string UserProducts => Profile + "/UserProducts";
        public static string ProfileAuthorize => Profile + "/Authorize";
        public static string ProfileWallet => Profile + "/UserWallet";
        public static string ProfileChat => Profile + "/Chat";
        #endregion

    }
}
