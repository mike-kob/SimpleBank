package api;

import sessions.Session;

public interface AtmAPIInterface {
    int getRemaining(Session session);

    void withdraw(Session session, int units);
}
