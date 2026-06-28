import { AppShell } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Outlet, useNavigate } from "react-router-dom";

import Header from "./Header";
import MobileMenu from "./MobileMenu";
import accountService from "../Pages/Account/accountService";
import { useUserStore } from "../stores/user.store";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface LayoutProps {}

// eslint-disable-next-line no-empty-pattern
function Layout({}: LayoutProps) {
  const [burgerOpened, { toggle: toggleBurger }] = useDisclosure();
  const navigate = useNavigate();
  const user = useUserStore();
  const authenticated = useUserStore((s) => s.authenticated);

  async function handleClickLogInOrOut() {
    if (authenticated) {
      await accountService.signOut();
      user.logout();
      navigate("/");
    } else {
      navigate("/account/signIn");
    }
  }

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{
        width: 300,
        breakpoint: "sm",
        collapsed: { desktop: true, mobile: !burgerOpened },
      }}
      padding="md"
    >
      <Header
        burgerOpened={burgerOpened}
        toggleBurger={toggleBurger}
        handleClickLogInOrOut={handleClickLogInOrOut}
        navigate={navigate}
        userAuthenticated={authenticated}
        userType={user.userType}
      />

      <MobileMenu navigate={navigate} toggleBurger={toggleBurger} />

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}

export default Layout;
