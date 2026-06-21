import { PropsWithChildren, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { UserType } from "../common/schemas";
import { useUserStore } from "../stores/user.store";

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
  }, [isUserType, navigate, notPermittedErrorMessage, permittedUserTypes]);

  return children;
}

export default ProtectedRoute;
