import { PropsWithChildren, useEffect } from "react";
import { UserType } from "../Account/SignIn/signIn.schema";
import { useUserStore } from "../stores/userStore";
import { useNavigate } from "react-router-dom";

interface ProtectedRouteProps {
  permittedUserTypes: Array<UserType>;
  notPermittedErrorMessage?: string;
}

function ProtectedRoute({
  permittedUserTypes,
  notPermittedErrorMessage,
  children,
}: PropsWithChildren<ProtectedRouteProps>) {
  const { isUserType } = useUserStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!permittedUserTypes.some(isUserType)) {
      navigate("/account/unauthorized", {
        state: { message: notPermittedErrorMessage },
      });
      return;
    }
  }, [isUserType]);

  return children;
}

export default ProtectedRoute;
