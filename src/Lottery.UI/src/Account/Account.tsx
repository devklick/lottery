import { useNavigate } from "react-router-dom";
import { useUserStore } from "../stores/user.store";
import { useEffect } from "react";

interface AccountProps {}

function Account({}: AccountProps) {
  const user = useUserStore();
  const navigate = useNavigate();
  useEffect(() => {
    if (!user.authenticated()) {
      navigate("/account/signIn");
    }
  });
  return "Hello from account";
}

export default Account;
