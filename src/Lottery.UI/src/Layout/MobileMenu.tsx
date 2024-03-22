import { AppShell, UnstyledButton } from "@mantine/core";

interface MobileMenuProps {
  navigate(path: string): void;
}

function MobileMenu({ navigate }: MobileMenuProps) {
  return (
    <AppShell.Navbar py="md" px={4}>
      <UnstyledButton onClick={() => navigate("/games")}>
        <span>Games</span>
      </UnstyledButton>
      <UnstyledButton onClick={() => navigate("/account")}>
        <span>Account</span>
      </UnstyledButton>
    </AppShell.Navbar>
  );
}
export default MobileMenu;
