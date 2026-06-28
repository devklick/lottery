import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import AccountDetails from "./AccountDetails/AccountDetails";
import YourEntries from "./YourEntries";
import YourWins from "./YourWins";
import Page from "../../components/Page";
import { useUserStore } from "../../stores/user.store";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface AccountProps {}

// eslint-disable-next-line no-empty-pattern
function Account({}: AccountProps) {
  const authenticated = useUserStore((s) => s.authenticated);
  const navigate = useNavigate();

  useEffect(() => {
    if (!authenticated) {
      navigate("/account/signIn");
    }
  }, [authenticated, navigate]);

  return (
    <Page title="Account">
      <AccountDetails authenticated={authenticated} />
      <YourEntries />
      <YourWins />
    </Page>
  );
}

export default Account;
