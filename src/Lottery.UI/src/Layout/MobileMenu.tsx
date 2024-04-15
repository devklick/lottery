import { AppShell, UnstyledButton } from "@mantine/core";

interface MobileMenuProps {
  navigate(path: string): void;
  toggleBurger(): void;
}

function MobileMenu({ navigate, toggleBurger }: MobileMenuProps) {
  function handleNavigate(path: string) {
    navigate(path);
    toggleBurger();
  }
  return (
    <AppShell.Navbar py="md" px={4}>
      <UnstyledButton onClick={() => handleNavigate("/games")}>
        <span>Games</span>
      </UnstyledButton>
      <UnstyledButton onClick={() => handleNavigate("/account")}>
        <span>Account</span>
      </UnstyledButton>
    </AppShell.Navbar>
  );
}
export default MobileMenu;
