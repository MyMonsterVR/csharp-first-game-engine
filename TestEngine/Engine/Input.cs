using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace TestEngine;

public class Input
{
    public static int GetInput(string mode)
    {
        var currentWindow = TestEngine.Window;
        if (!currentWindow.isInFocus) return 0;
        switch (mode)
        {
            case "Vertical":
                if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) != 0)
                {
                    return -1;
                }

                if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) != 0)
                {
                    return 1;
                }

                break;
            case "Horizontal":
                if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) != 0)
                {
                    return -1;
                }

                if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) != 0)
                {
                    return 1;
                }

                break;
        }

        return 0;
    }
}