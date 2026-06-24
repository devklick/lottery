import Account from "./Account";
import ConfirmEmailChange from "./ConfirmEmailChange";
import ForgotPassword from "./ForgotPassword";
import ResetPassword from "./ResetPassword";
import SignIn from "./SignIn";
import SignUp from "./SignUp";
import Unauthorized from "./Unauthorized";
import VerifyEmail from "./VerifyEmail";

export default {
  // Main page for this path
  Page: Account,

  // Pages for child paths
  SignIn,
  SignUp,
  Unauthorized,
  VerifyEmail,
  ConfirmEmailChange,
  ForgotPassword,
  ResetPassword,
};
