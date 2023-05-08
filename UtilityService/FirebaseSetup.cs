using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UtilityService
{
    public static class FirebaseSetup
    {
        public static async void SetupFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile(configuration["Firebase:CredentialsPath"])
            //});
            //services.AddSingleton<FirebaseStorage>(FirebaseStorage.DefaultInstance);

            //var auth = new FirebaseAuthProvider(new FirebaseConfig(configuration["Firebase:API_KEY"]));
            //var token = await auth.SignInWithEmailAndPasswordAsync(configuration["Firebase:EMAIL"], configuration["Firebase:PASSWORD"]);
            //var firebaseStorage = new FirebaseStorage(configuration["Firebase:BUCKET_NAME"], new FirebaseStorageOptions
            //{
            //    AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken),
            //    ThrowOnCancel = true
            //});
            
            //services.AddSingleton<IFirebaseStorage>(new FirebaseStorage(configuration["Firebase:BUCKET_NAME"], new FirebaseStorageOptions
            //{
            //    AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken),
            //    ThrowOnCancel = true
            //}));
        }

    }
}