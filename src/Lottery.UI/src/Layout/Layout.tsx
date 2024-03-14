import { AppShell, Burger, Group, UnstyledButton } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Outlet, useNavigate } from "react-router-dom";

interface LayoutProps {}
function Layout({}: LayoutProps) {
  const [opened, { toggle }] = useDisclosure();
  const nav = useNavigate();
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
              <UnstyledButton onClick={() => nav("/account")}>
                <span>Account</span>
              </UnstyledButton>
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
