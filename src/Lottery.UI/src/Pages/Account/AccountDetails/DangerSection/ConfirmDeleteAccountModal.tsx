import { Button, Modal, Stack, Text, useMantineTheme } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck, IconXboxX } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";

import { useDeleteAccount } from "./hooks";
import { useUserStore } from "../../../../stores/user.store";
import ConfirmPassword from "../../ConfirmPasswordModal/ConfirmPassword";

interface ConfirmDeleteAccountModalProps {
  closeModal(): void;
  opened: boolean;
}
export default function ConfirmDeleteAccountModal({
  closeModal,
  opened,
}: ConfirmDeleteAccountModalProps) {
  const [passwordRequired, { open: setPasswordRequired }] = useDisclosure();
  const logout = useUserStore((s) => s.logout);
  const navigate = useNavigate();
  const { colors } = useMantineTheme();

  const deleteAccount = useDeleteAccount({
    onReAuthRequired: setPasswordRequired,
    onSuccess: onAccountDeleted,
  });

  function onPasswordFailed() {
    notifications.show({
      title: "Unable to delete account",
      message: "Password verification failed",
      icon: <IconXboxX />,
      color: colors.red[5],
      autoClose: 5000,
    });
  }

  function onPasswordSuccess() {
    deleteAccount.mutate();
  }

  function onAccountDeleted() {
    closeModal();
    notifications.show({
      title: "Account Deleted",
      message: "You can no longer log in to your account",
      icon: <IconCircleCheck />,
      color: "green",
      autoClose: 7000,
    });
    logout();
    navigate("/home");
  }

  return (
    <Modal title="Delete your account" onClose={closeModal} opened={opened}>
      <Stack align="center" p={"lg"}>
        <Text>Are you sure you want to delete your account?</Text>
        <Text>This action cannot be undone</Text>
        {!passwordRequired && (
          <Button mt={"lg"} color="red" onClick={() => deleteAccount.mutate()}>
            Delete
          </Button>
        )}
        {passwordRequired && (
          <ConfirmPassword
            reason="Please confirm your password to delete your account"
            onFailure={onPasswordFailed}
            onSuccess={onPasswordSuccess}
          />
        )}
      </Stack>
    </Modal>
  );
}
