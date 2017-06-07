# Quick Check-In for Trakt.tv

## Description

This Android client for Trakt.tv allows browsing movies and shows, managing your lists and checking in.

## How to Run

In order to build the project by yourself you need to obtain a Trakt.tv API key and create the file QuickCheckIn/Resources/values/ApiKey.xml with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <string name="ClientId">Your client ID goes here</string>
    <string name="ClientSecret">Your client secret goes here</string>
</resources>
```