package utils;

import javax.swing.*;
import java.awt.*;

public class LocationHelper {

    public static Point centerLocation(JLayeredPane jp, JComponent jc){
        Dimension dm = jp.getSize();
        int x = (dm.width - jc.getWidth()) / 2;
        int y = (dm.height - jc.getHeight()) / 2;

        return new Point(x, y);
    }
}
