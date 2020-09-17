//////////////////////////////////////////////////////////////////////////////////////////////////////
//	Settings File
//	Copyright (C) 2004-6  Erik Ylvisaker
//						
//	
//	This program is free software; you can redistribute it and/or
//	modify it under the terms of the GNU General Public License
//	as published by the Free Software Foundation; either version 2
//	of the License, or (at your option) any later version.
//	
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//	
//	You should have received a copy of the GNU General Public License
//	along with this program; if not, write to the Free Software
//	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
//	The original author can be contacted at:
//  Erik Ylvisaker <smokeyrobinsen@hotmail.com>
//

using System;
using System.Collections.Generic;
using System.IO;

class SetValStruct 
{
	public string setting;
    public string sValue;
    public int iValue;
    public double dValue;

    public List<int> multivalue = new List<int>();
    public List<string> multivalueString = new List<string>();

    public override string ToString()
    {
        return string.Format("{0} = {1}", setting, sValue);
    }
};


class CSettingsFile
{
    string mFileName;
    string mData;
    bool mIsValid;


    Dictionary<string, SetValStruct> mSettings = new Dictionary<string, SetValStruct>();
    List<SetValStruct> mSettingsVector = new List<SetValStruct>();

    List<string> mSections = new List<string>();


    /// <summary>
    /// Initializes a blank settings file.  Once initialized, call OpenFile to actuall
    ///	open a settings file.
    /// </summary>
    public CSettingsFile()
    {
        mIsValid = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    //	CSettingsFile(string fileName) : mIsValid(false)
    //
    //	Initializes a settings file.  Automatically opens the file passed in.
    //
    //	Arguments:		fileName: name of file to open.
    //
    //	Return Value:	none  
    //
    public CSettingsFile(string fileName)
        : this()
    {
        OpenFile(fileName);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    //	~CSettingsFile()
    //
    //	Destroys the settings file and frees any memory used.
    //
    //	Arguments:		none
    //
    //	Return Value:	none  
    //
    ~CSettingsFile()
    {
        Clear();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    //	void clear()
    //
    //	Frees any memory used by the structures storing the settings.
    //
    //	Arguments:		none
    //
    //	Return Value:	none  
    //
    public void Clear()
    {
        mSettingsVector.Clear();
        mSettings.Clear();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    //	bool IsValid() 
    //
    //	Tells whether or not the file was read properly
    //
    //	Arguments:		none
    //
    //	Return Value:	true if the object contains useful data.
    //
    public bool IsValid
    {
        get
        {
            return mIsValid;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    //	bool OpenFile(string fileName)
    //
    //	Opens a file, reads it in, and parses it into the settings.
    //	This function should be able to be called multiple times to combine multiple
    //	settings files into one data structure in memory. (untested)
    //
    //	Arguments:		fileName: name of the file to read.
    //
    //	Return Value:	true if the object contains useful data.
    //
    public bool OpenFile(string fileName)
    {
        mFileName = fileName;

        using (StreamReader infile = new StreamReader(fileName))
        {

            mIsValid = false;

            mData = infile.ReadToEnd();

            ParseFile();


            return IsValid;
        }
    }

    public void Save()
    {

        mSettingsVector.Clear();
        foreach (KeyValuePair<string, SetValStruct> kvp in mSettings)
            mSettingsVector.Add(kvp.Value);

        mSettingsVector.Sort(Sorter);

        using (StreamWriter writer = new StreamWriter(mFileName))
        {
            string lastCategory = "";

            for (int i = 0; i < mSettingsVector.Count; i++)
            {
                SetValStruct s = mSettingsVector[i];
                string settingname;

                if (s.setting.Contains("."))
                {
                    string cat = s.setting.Substring(0, s.setting.IndexOf("."));

                    if (cat.Equals(lastCategory, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        lastCategory = cat;

                        writer.WriteLine();
                        writer.WriteLine("[{0}]", lastCategory);
                    }

                    settingname = s.setting.Replace(cat + ".", "");
                }
                else settingname = s.setting;

                writer.WriteLine("{0} = {1}", settingname, s.sValue);

            }
        }
    }

    private static int Sorter(SetValStruct a, SetValStruct b)
    {
        if (a.setting.Contains(".") && b.setting.Contains(".") == false)
            return 1;
        if (b.setting.Contains(".") && a.setting.Contains(".") == false)
            return -1;

        return a.setting.CompareTo(b.setting);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	void ParseFile()
    //
    //	Parses the file.  The Settings file parser does the following:
    //		Break up the file into lines, separated by carriage returns.
    //		Throw out lines that begin with a '#' which are considered comment lines.
    //		Look for section headers, which have format: "[section]"
    //		Split a settings the line on the = sign and store the setting for "Settingname = value" type lines.
    //			The names of settings are stored as "section.settingname" if there a section was read before this
    //			line, or just "settingname" otherwise.
    //		Reads values that list multiple values.  If the following string is read: "4, 6, 10, 12..17" 
    //			the values that would be stored are 4, 6, 10, 12, 13, 14, 15, 16, and 17.
    //
    //	Arguments:		fileName: name of the file to read.
    //
    //	Return Value:	true if the object contains useful data.
    //
    void ParseFile()
    {
        SetValStruct s;

        string objectName = "";
        string set, value;

        int linesRead = 0;
        bool inScript = false;
        string script = "";

        Clear();


        //
        // find the next carriage return
        string[] lines = (mData + "\n").Split('\n');

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex];

            ++linesRead;

            line = line.Replace("\r", "").Replace("\n", "").Trim();


            if (string.IsNullOrEmpty(line) || line[0] == '#')	/// this is a comment in a mSettings file
                continue;		// go to the next part of the for loop

            if (line.Length < 3) // this is a bogus line (of the file we are reading, not in the code)
            {
                System.Diagnostics.Debug.WriteLine("Unable to understand line #" + linesRead + ":\n" + line);
                continue;
            }

            // check to see if there's inline script in there.
            if (line.Trim().ToLower() == "<script>" && !inScript)
            {
                inScript = true;

                script = "";

                continue;

            }
            // check to see if we are ending a script
            else if (line.Trim().ToLower() == "</script>" && inScript)
            {
                // well, we should save the script that's being read.
                inScript = false;

                s = new SetValStruct();


                if (objectName != "")
                    s.setting = objectName + ".script";
                else
                    s.setting = "script";

                s.sValue = script;

                continue;
            }
            else if (inScript)
            {
                // we're in the script, but not at the end, so save the line.
                script += line + "\n";

                continue;
            }

            // look for a bracket
            int lbracket = line.IndexOf('[');
            int rbracket = line.IndexOf(']');

            if (lbracket != -1 && rbracket != -1)
            {
                objectName = line.Substring(lbracket + 1, (rbracket - lbracket) - 1).Trim().ToLower();

                if (mSections.Contains(objectName) == false)
                    mSections.Add(objectName);

                continue;
            }
            else if (lbracket != -1)
            {
                System.Diagnostics.Debug.Print("Unable to understand line: {0}\n{1}\n", linesRead, line);
                continue;
            }

            // now find an equals sign
            int equals = line.IndexOf('=');

            if (equals == -1)
            {
                // no setting?
                System.Diagnostics.Debug.Print("No setting on line: {0}\n{1}\n", linesRead, line);
            }
            else
            {
                // we found an equals sign, so break this up into the setting and the value
                s = new SetValStruct();

                string[] items = line.Split('=');
                set = items[0];
                value = items[1];

                if (objectName != "")
                    s.setting = objectName + "." + set.Trim().ToLower();
                else
                    s.setting = set.Trim().ToLower();

                s.sValue = value.Trim();
                int.TryParse(value, out s.iValue);
                double.TryParse(value, out s.dValue);

                ////////////////////////////////////////////////////////////////
                // check to see if this is multivalued.
                if (s.sValue.Contains("..") || s.sValue.Contains(","))
                {
                    // yes, it's multivalued.  now we need to split up the values and 
                    // build an array

                    set = s.sValue;

                    string[] values = s.sValue.Split(',');
                    List<string> listValues = new List<string>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i].Contains(".."))
                        {
                            string[] range = values[i].Split(new string[] { ".." }, StringSplitOptions.None);

                            if (range.Length != 2)
                            {
                                System.Diagnostics.Debug.Print("Unable to understand line: {0}\n{1}\n", 
                                    linesRead, line);

                                break;
                            }

                            int low = int.Parse(range[0]);
                            int high = int.Parse(range[1]);

                            for (int sdf = low; sdf <= high; sdf ++)
                            {
                                listValues.Add(sdf.ToString().Trim());
                            }

                        }
                        else
                            listValues.Add(values[i].Trim());
                    }

                    // now store them
                    for (int i = 0; i < listValues.Count; i++)
                    {
                        s.multivalueString.Add(listValues[i]);

                        int myvalue;

                        if (int.TryParse(listValues[i], out myvalue))
                        {
                            s.multivalue.Add(myvalue);
                        }

                    }

                }

                // store it.
                mSettings[s.setting] = s;
                mSettingsVector.Add(s);
            }



        }

        mIsValid = true;

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	SetValStruct GetSettingStruct(string setting) 
    //
    //	Private member used to lookup the setting name passed in.  Not really necessary with the 
    //	use of a map, but oh well.
    //
    //
    SetValStruct GetSettingStruct(string setting)
    {
        return mSettings[setting.ToLower()];
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	string ReadValueS(string setting, string defaultValue) 
    //
    //	Looks up the setting passed in and returns its string value.
    //	This is the part of the setting after the equals sign, exactly as typed, minus any
    //	leading or trailing spaces.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //
    //	Return Value:	a string object which contains the value.
    //
    public string ReadString(string setting, string defaultValue)
    {
        try
        {
            SetValStruct s = GetSettingStruct(setting);
            return s.sValue;
        }
        catch 
        {
            return defaultValue;
        }


    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	int ReadValueI(string setting, int defaultValue) 
    //
    //	Looks up the setting passed in and returns its integer value.
    //	This is the part of the setting after the equals sign, converted to an integer.
    //	It should be zero if there was a string and no number.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //
    //	Return Value:	an int which contains the value of the setting.
    //
    public int ReadInteger(string setting, int defaultValue)
    {
        try
        {
            SetValStruct s = GetSettingStruct(setting);
            return s.iValue;
        }
        catch
        {
            return defaultValue;
        }


    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	double ReadValueD(string setting, double defaultValue) 
    //
    //	Looks up the setting passed in and returns its double value.
    //	This is the part of the setting after the equals sign, converted to an double.
    //	It should be zero if there was a string and no number.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //
    //	Return Value:	a double which contains the value of the setting.
    //
    public double ReadDouble(string setting, double defaultValue)
    {
        try
        {
            SetValStruct s = GetSettingStruct(setting);
            return s.dValue;
        }
        catch
        {
            return defaultValue;
        }


    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	int ReadMultiValueCount(string setting) 
    //
    //	Looks up the setting passed in and returns the number of multivalues that setting contains.
    //	This is for settings which list a series of values.  This function returns how many, so that
    //	the calling function can allocate an array of ints that size.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //
    //	Return Value:	an int which contains the number of values there are.
    //
    public int ReadMultiValueCount(string setting)
    {
        try
        {
            SetValStruct s = GetSettingStruct(setting);
            return s.multivalue.Count;
        }
        catch
        {
            return 0;
        }


    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	void ReadMultiValueI(string setting, int *array, int arraySize) 
    //
    //	Looks up the setting passed in and copies the values into the array.
    //	This is for settings which list a series of values.  This function will copy the
    //	series of values into the array pointer passed in.  The values are unsorted.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //					array:	 pointer to memory to fill
    //					arraySize: maximum number of values to write to that pointer.
    //
    //	Return Value:	an int which contains the number of values there are.
    //
    public int[] ReadIntegerArray(string setting)
    {
        List<int> array;

        try
        {
            SetValStruct s = GetSettingStruct(setting);

            array = new List<int>();
            array.AddRange(s.multivalue);
        }
        catch
        {
            return new int[0];
        }

        return array.ToArray();
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //	string ReadMultiValueS(string setting, int index) 
    //
    //	Looks up the setting passed in returns the string index of the multivalued string requested.
    //
    //	Arguments:		setting: name of the setting to lookup.
    //					index: the number of the string to get
    //
    //	Return Value:	the actual string value set in the settings file.
    //
    public string[] ReadStringArray(string setting)
    {
        try
        {
            SetValStruct s = GetSettingStruct(setting);

            return s.multivalueString.ToArray();
        }
        catch
        {
            return new string[0];
        }

    }

    public void WriteString(string setting, string value)
    {
        SetValStruct s;

        if (mSections.Contains(setting.ToLower()))
        {
            s = mSettings[setting.ToLower()];
        }
        else
        {
            s = new SetValStruct();
            s.setting = setting;
            mSettings.Add(setting.ToLower(), s);
        }

        s.sValue = value;
    }
    public void WriteInteger(string setting, int value)
    {
        SetValStruct s;

        if (mSections.Contains(setting.ToLower()))
        {
            s = mSettings[setting.ToLower()];
        }
        else
        {
            s = new SetValStruct();
            s.setting = setting;
            mSettings.Add(setting.ToLower(), s);
        }

        s.sValue = value.ToString();
        s.iValue = value;
    }
    public void WriteDouble(string setting, double value)
    {
        SetValStruct s;

        if (mSections.Contains(setting.ToLower()))
        {
            s = mSettings[setting.ToLower()];
        }
        else
        {
            s = new SetValStruct();
            s.setting = setting;
            mSettings.Add(setting.ToLower(), s);
        }

        s.sValue = value.ToString();
        s.dValue = value;
    }
    public void WriteStringArray(string setting, string[] values)
    {
        SetValStruct s;

        if (mSettings.ContainsKey(setting.ToLower()))
        {
            s = mSettings[setting.ToLower()];
        }
        else
        {
            s = new SetValStruct();
            s.setting = setting;
            mSettings.Add(setting.ToLower(), s);
        }

        s.sValue = string.Join(", ", values);
        s.multivalueString.Clear();
        s.multivalueString.AddRange(values);
    }
    public int SectionCount
    {
        get
        {
            return mSections.Count;
        }
    }

    public string getSection(int index)
    {
        return mSections[index];
    }


    
    public string ReadString(string setting)
    {
        return ReadString(setting, "");
    }
    public int ReadInteger(string setting)
    {
        return ReadInteger(setting, 0);
    }
    public bool ReadBoolean(string setting)
    {
        return ReadBoolean(setting, false);
    }
    public bool ReadBoolean(string setting, bool defaultValue)
    {
        return (ReadInteger(setting, 0) != 0) ? true : false;
    }
    public double ReadDouble(string setting)
    {
        return ReadDouble(setting, 0);
    }
    


    ////////////////////////////////////////////////////////////////
    // Array type functions
    //
    /*
    int SettingsCount()
    {
        return mSettings.Count;
    }

    string GetSetting(int index)
    {
        return (mSettings.begin()..setting;
    }

    string GetValue(int index)
    {
        return mSettings.GetAt(iSettings[index]).sValue;
    }

    int GetMultiValueCount(int index)
    {
        return mSettings.GetAt(iSettings[index]).multivalueCount;
    }

    void GetMultiValue(int index, int *array, int arraySize)
    {
        SetValStruct s = mSettings.GetAt(iSettings[index]);

        memcpy(array, s.multivalue, MIN(arraySize, s.multivalueCount) * sizeof(int));
    }
    */
    //
    //// end array type stuff






}


