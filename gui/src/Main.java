import javax.swing.*;

public class Main {
    public static final JFrame frame = new JFrame();
    public static final JLayeredPane lp = frame.getLayeredPane();

    public static void main(String[] args) {
        initWindow();
        Session s = new Session(lp);
    }

    public static void initWindow() {
        frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
        frame.setVisible(true);
        frame.setLayout(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setResizable(true);
    }
}
