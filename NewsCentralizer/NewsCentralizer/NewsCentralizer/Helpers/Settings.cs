﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace NewsCentralizer.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private const string UserIdKey = "userid_key";
        private static readonly string UserIdDefault = string.Empty;

        private const string AuthTokenKey = "authtoken_key";
        private static readonly string AuthTokenDefault = string.Empty;

        private const string LoginProviderKey = "loginprovider_key";
        private const string LoginProviderDefault = "0";


        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault(AuthTokenKey, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue(AuthTokenKey, value); }
        }

        public static string LoginProvider
        {
            get { return AppSettings.GetValueOrDefault(LoginProviderKey, LoginProviderDefault); }
            set { AppSettings.AddOrUpdateValue(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
    }

    public static class Constants
    {
        public const string AppUrl = "https://maratonaxamarinfel.azurewebsites.net/";
    }
}