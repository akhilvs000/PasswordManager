# PasswordManager
This release intended to share a simple password manager app user to store a basic account details.

## Supported platforms : iOS, Android


## Libraries used
1. Xamarin.Forms(5.0.0.2012)
2. Prism.DryIoc.Forms(8.1.97)
3. Realm database(10.10.0)

## Supported application language
1. English
2. German

## Features

1. Set a master password
2. Store account details(website name, url, username and encrypted password)
3. View account details
4. Edit/Delete account details
5. Login with a password
6. Change app language (en, de)

### Notes
1. Master passwords are not stored, on the first launch, when user sets the master password, it adds a dummy account details item to the database
2. Dummy account detail password is encrypted with the master password text entered
3. While app relaunch, it always ask for password to relogin, which trys to decrypt the dummy account with entered login password. If success, go to account list page, otherwise shows wrong password error
4. While app comes from background state to foreground, Always ask to relogin
5. Used AES algorithm for encryption
 
