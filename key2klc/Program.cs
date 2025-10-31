using System;
using System.Collections;
using System.IO;
using System.Reflection;

class KeyboardExtractor
{
    static Object InvokeNonPublicStaticMethod(Type t, String name,
            Object[] args)
    {
        return t.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic)
            .Invoke(null, args);
    }

    static Object GetNonPublicProperty(Object o, String propertyName)
    {
        return o.GetType().GetField(propertyName,
                BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(o);
    }

    static void SetNonPublicField(Object o, String propertyName, Object v)
    {
        o.GetType().GetField(propertyName,
                BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(o, v);
    }

    [STAThread]
    public static void Main()
    {
        Console.WriteLine("Keyboard Extractor...");

        KeyboardExtractor ke = new KeyboardExtractor();
        ke.extractAll();

        Console.WriteLine("Done.");
    }

    Assembly msklcAssembly;
    Type utilitiesType;
    Type keyboardType;
    String baseDirectory;

    public KeyboardExtractor()
    {
        msklcAssembly = Assembly.LoadFile("C:\\Program Files (x86)\\Microsoft Keyboard Layout Creator 1.4\\MSKLC.exe");
        utilitiesType = msklcAssembly.GetType("Microsoft.Globalization.Tools.KeyboardLayoutCreator.Utilities");
        keyboardType = msklcAssembly.GetType("Microsoft.Globalization.Tools.KeyboardLayoutCreator.Keyboard");

        baseDirectory = Directory.GetCurrentDirectory();
    }

    public void extractAll()
    {
        DateTime startTime = DateTime.UtcNow;

        SortedList keyboards = InvokeNonPublicStaticMethod(utilitiesType, 
            "KeyboardsOnMachine", new Object[] { false }) as SortedList;

        int i = 0;
        foreach (DictionaryEntry e in keyboards)
        {
            i += 1;
            Object k = e.Value;

            String name = (String)GetNonPublicProperty(k, "m_stLayoutName");
            String layoutHexString = ((UInt32)GetNonPublicProperty(k, "m_hkl"))
                .ToString("X");

            if (!(name.ToLower().Contains("slovak") || name.ToLower().Contains("czech"))) 
                continue;

            Console.WriteLine(
                    "Saving {0} {1}, keyboard {2} of {3}",
                    layoutHexString, name, i, keyboards.Count);

            SaveKeyboard(name, layoutHexString);
        }

        Console.WriteLine("{0} elapsed", DateTime.UtcNow - startTime);
    }

    private void SaveKeyboard(String name, String layoutHexString)
    {
        Object k = 
            keyboardType
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
            .Invoke(new Object[] {
                new String[] {"", layoutHexString}, false
            });

        SetNonPublicField(k, "m_fSeenOrHeardAboutPropertiesDialog", true);
        SetNonPublicField(k, "m_stKeyboardTextFileName",
            String.Format("{0}\\{1} {2}.klc",
            baseDirectory, layoutHexString, name));

        ((IDisposable)k).Dispose();
    }
}
