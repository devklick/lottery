import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useGetAccount } from "./account.hooks";
import AccountDetails from "./AccountDetails/AccountDetails";
import YourEntries from "./YourEntries";
import YourWins from "./YourWins";
import Page from "../components/Page";
import { useUserStore } from "../stores/user.store";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface AccountProps {}

// eslint-disable-next-line no-empty-pattern
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
