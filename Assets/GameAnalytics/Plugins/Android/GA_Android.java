package com.yourcompany.yourgame;

import com.unity3d.player.UnityPlayerActivity;

import android.app.Activity;
import android.content.Context;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.util.Log;
import java.io.File;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
 
import android.os.Bundle;

public class GA_Android extends UnityPlayerActivity
{
    private static android.content.ContentResolver contentResolver;
     
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);

        contentResolver = getContentResolver();
    }
     
    public static String GetDeviceId()
    {
        // Get the device ID
        return Secure.getString(contentResolver, Secure.ANDROID_ID);
    }
}