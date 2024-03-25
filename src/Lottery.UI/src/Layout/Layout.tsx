import {
  AppShell,
  useComputedColorScheme,
  useMantineColorScheme,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Outlet, useNavigate } from "react-router-dom";
import { useUserStore } from "../stores/user.store";
import accountService from "../Account/accountService";
import Header from "./Header";
import MobileMenu from "./MobileMenu";

interface LayoutProps {}

function Layout({}: LayoutProps) {
  const [burgerOpened, { toggle: toggleBurger }] = useDisclosure();
  const navigate = useNavigate();
  const user = useUserStore();
  const { toggleColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme();

  async function handleClickLogInOrOut() {
    if (user.authenticated()) {
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
        colorScheme={computedColorScheme}
        toggleColorScheme={toggleColorScheme}
        handleClickLogInOrOut={handleClickLogInOrOut}
        navigate={navigate}
        userAuthenticated={user.authenticated()}
        userType={user.userType}
      />

      <MobileMenu navigate={navigate} />

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}

export default Layout;
