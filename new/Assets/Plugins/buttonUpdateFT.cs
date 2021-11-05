using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class ConnectedForceTube
{
    public List<string> Connected;
}





public class buttonUpdateFT : MonoBehaviour
{
    public Dropdown m_db_toUpdate;
    List<string> m_DropOptions=null;


    public void PutainDeButton()
    {
        
        //Clear the old options of the Dropdown menu
        m_db_toUpdate.ClearOptions();
        string sText = ForceTubeVRInterface.ListConnectedForceTube();

        //ChannelList channe
        //ChannelList channe
        if (sText != null)
        {
            ConnectedForceTube FTList = JsonUtility.FromJson<ConnectedForceTube>(sText);

            m_DropOptions = new List<string>();

            foreach (string  sName in FTList.Connected)
            {
                m_DropOptions.Add(sName);
            }


            //Add the options created in the List above
            m_db_toUpdate.AddOptions(m_DropOptions);
            m_db_toUpdate.value = -1;
            m_db_toUpdate.RefreshShownValue();
        }
    }
}
