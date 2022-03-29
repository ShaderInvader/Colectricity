

public static class ControllerInfo
{
    public enum controllerEnum
    {
        KEYBOARD, BOTH, PAD,
    }

    public static string getObjectName(controllerEnum controllerEnum)
    {
        switch(controllerEnum)
        {
            case controllerEnum.KEYBOARD:
                return "pc_only";

            case controllerEnum.PAD:
                return "pad+pad";

            case controllerEnum.BOTH:
                return "pc+pad";
        }
        return null;
    }

    public static controllerEnum controllerPick = controllerEnum.KEYBOARD;
}
