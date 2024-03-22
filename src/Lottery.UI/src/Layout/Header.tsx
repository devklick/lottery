import {
  AppShell,
  Burger,
  Group,
  Menu,
  Switch,
  UnstyledButton,
} from "@mantine/core";
import { IconLogin, IconMoon, IconSun } from "@tabler/icons-react";
import { UserType } from "../Account/SignIn/signIn.schema";

interface HeaderProps {
  burgerOpened: boolean;
  toggleBurger(): void;
  navigate(path: string): void;
  handleClickLogInOrOut(): void;
  userAuthenticated: boolean;
  userType: UserType;
  colorScheme: ColorScheme;
  toggleColorScheme(): void;
}

function Header({
  burgerOpened,
  toggleBurger,
  navigate,
  handleClickLogInOrOut,
  userAuthenticated,
  userType,
  colorScheme,
  toggleColorScheme,
}: HeaderProps) {
  return (
    <AppShell.Header>
      <Group h="100%" px="md">
        <Burger
          opened={burgerOpened}
          onClick={toggleBurger}
          hiddenFrom="sm"
          size="sm"
        />
        <Group justify="space-between" style={{ flex: 1 }}>
          <Group></Group>
          <Group ml="xl" gap={20} visibleFrom="sm">
            <Menu
              trigger="hover"
              openDelay={100}
              loop={false}
              withinPortal={false}
              trapFocus={false}
            >
              <Menu.Target>
                <UnstyledButton onClick={() => navigate("/games")}>
                  <span>Games</span>
                </UnstyledButton>
              </Menu.Target>
              <Menu.Dropdown>
                {userAuthenticated && userType == "Admin" && (
                  <Menu.Item onClick={() => navigate("/games/create")}>
                    Create Game
                  </Menu.Item>
                )}
              </Menu.Dropdown>
            </Menu>
            <Menu
              trigger="hover"
              openDelay={100}
              loop={false}
              withinPortal={false}
              trapFocus={false}
            >
              <Menu.Target>
                <UnstyledButton onClick={() => navigate("/account")}>
                  <span onClick={() => navigate("/account")}>Account</span>
                </UnstyledButton>
              </Menu.Target>
              <Menu.Dropdown>
                <Menu.Item
                  leftSection={<IconLogin />}
                  onClick={handleClickLogInOrOut}
                >
                  {userAuthenticated ? "Log Out" : "Log In"}
                </Menu.Item>
                <Menu.Divider />
                <Menu.Label>Application</Menu.Label>
                <Menu.Item
                  onClick={toggleColorScheme}
                  leftSection={
                    <Switch
                      onLabel={<IconSun />}
                      offLabel={<IconMoon />}
                      checked={colorScheme === "light"}
                      style={{ pointerEvents: "none" }}
                    />
                  }
                >{`Theme`}</Menu.Item>
              </Menu.Dropdown>
            </Menu>
          </Group>
        </Group>
      </Group>
    </AppShell.Header>
  );
}

export default Header;
