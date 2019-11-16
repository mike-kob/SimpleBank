package api;

import sessions.Session;

public interface UserAPIInterface {

    Session startSession();

    boolean login(Session session);



}
