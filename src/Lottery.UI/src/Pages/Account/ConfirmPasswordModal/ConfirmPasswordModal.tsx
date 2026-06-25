import { Modal } from "@mantine/core";

import ConfirmPassword from "./ConfirmPassword";

interface ConfirmPasswordModalProps {
  onSuccess(): void;
  onFailure(): void;
  onCancel(): void;
  opened: boolean;
  reason: string;
}

export default function ConfirmPasswordModal({
  onCancel,
  onFailure,
  onSuccess,
  opened,
  reason
}: ConfirmPasswordModalProps) {
  return (
    <Modal title="Confirm Password" opened={opened} onClose={onCancel}>
      <ConfirmPassword reason={reason} onFailure={onFailure} onSuccess={onSuccess} />
    </Modal>
  );
}
