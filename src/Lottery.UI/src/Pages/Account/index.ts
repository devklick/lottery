import Account from "./Account";
import ConfirmEmail from "./ConfirmEmail";
import ConfirmEmailChange from "./ConfirmEmailChange";
import SignIn from "./SignIn";
import SignUp from "./SignUp";
import Unauthorized from "./Unauthorized";

export default {
  // Main page for this path
  Page: Account,

  // Pages for child paths
  SignIn,
  SignUp,
  Unauthorized,
  ConfirmEmail,
  ConfirmEmailChange,
};
