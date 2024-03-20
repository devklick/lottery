import {
  AppShell,
  Burger,
  Group,
  Menu,
  Switch,
  UnstyledButton,
  useComputedColorScheme,
  useMantineColorScheme,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Outlet, useNavigate } from "react-router-dom";
import { IconLogin, IconSun, IconMoon } from "@tabler/icons-react";
import { useUserStore } from "../stores/user.store";

interface LayoutProps {}

function Layout({}: LayoutProps) {
  const [opened, { toggle }] = useDisclosure();
  const nav = useNavigate();
  const user = useUserStore();
  const { toggleColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme();
  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{
        width: 300,
        breakpoint: "sm",
        collapsed: { desktop: true, mobile: !opened },
      }}
      padding="md"
    >
      <AppShell.Header>
        <Group h="100%" px="md">
          <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
          <Group justify="space-between" style={{ flex: 1 }}>
            {/* <MantineLogo size={30} /> */}
            <div />
            <Group ml="xl" gap={20} visibleFrom="sm">
              <UnstyledButton onClick={() => nav("/games")}>
                <span>Games</span>
              </UnstyledButton>
              <Menu trigger="hover" openDelay={100} closeDelay={400}>
                <Menu.Target>
                  <UnstyledButton onClick={() => nav("/account")}>
                    <span onClick={() => nav("/account")}>Account</span>
                  </UnstyledButton>
                </Menu.Target>
                <Menu.Dropdown>
                  <Menu.Item leftSection={<IconLogin />}>
                    {user.authenticated ? "Log Out" : "Log In"}
                  </Menu.Item>
                  <Menu.Divider />
                  <Menu.Label>Application</Menu.Label>
                  <Menu.Item
                    onClick={toggleColorScheme}
                    leftSection={
                      <Switch
                        onLabel={<IconSun />}
                        offLabel={<IconMoon />}
                        checked={computedColorScheme === "light"}
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

      <AppShell.Navbar py="md" px={4}>
        <UnstyledButton onClick={() => nav("/games")}>
          <span>Games</span>
        </UnstyledButton>
        <UnstyledButton onClick={() => nav("/account")}>
          <span>Account</span>
        </UnstyledButton>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}

export default Layout;
