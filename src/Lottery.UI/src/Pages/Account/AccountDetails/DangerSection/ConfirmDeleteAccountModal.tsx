import { Button, Modal, Stack, Text } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useNavigate } from "react-router-dom";

import { useDeleteAccount } from "./hooks";
import { notifyError, notifySuccess } from "../../../../common/notifications";
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

  const deleteAccount = useDeleteAccount({
    onReAuthRequired: setPasswordRequired,
    onSuccess: onAccountDeleted,
  });

  function onPasswordFailed() {
    notifyError({
      title: "Password verification failed",
      message: "Unable to delete your account",
    });
  }

  function onPasswordSuccess() {
    deleteAccount.mutate();
  }

  function onAccountDeleted() {
    closeModal();
    notifySuccess({
      title: "Account Deleted",
      message: "You can no longer log in to your account",
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
