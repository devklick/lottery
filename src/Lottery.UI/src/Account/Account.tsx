import { useNavigate } from "react-router-dom";
import { useUserStore } from "../stores/user.store";
import { useEffect } from "react";
import { useGetAccount } from "./account.hooks";
import Page from "../components/Page";
import AccountDetails from "./AccountDetails/AccountDetails";
import YourEntries from "./YourEntries";
import YourWins from "./YourWins";

interface AccountProps {}

function Account({}: AccountProps) {
  const user = useUserStore();
  const navigate = useNavigate();
  useEffect(() => {
    if (!user.authenticated()) {
      navigate("/account/signIn");
    }
  }, []);

  const accountQuery = useGetAccount({ enabled: user.authenticated() });

  return (
    <Page title="Account">
      {accountQuery.data && <AccountDetails {...accountQuery.data} />}
      <YourEntries />
      <YourWins />
    </Page>
  );
}

export default Account;
