using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZerglingUnityPlugins.WindowsManagerAsync.Scripts.Basics
{
    public interface IWindowSetup
    {
    }

    public abstract class WindowSetup : IWindowSetup
    {
    }


    public interface IWindowSetupEmpty : IWindowSetup
    {
    }

    public class WindowSetupEmpty : WindowSetup, IWindowSetupEmpty
    {
        public static readonly WindowSetupEmpty Instance = new WindowSetupEmpty();
    }
}


