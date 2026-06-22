import { Button, Modal, PasswordInput, Stack } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { useEffect } from "react";

import {
  ConfirmPasswordRequestBody,
  confirmPasswordRequestBodySchema,
} from "./confirmPassword.schema";
import { useConfirmPassword } from "../account.hooks";

interface ConfirmPasswordModalProps {
  onSuccess(): void;
  onFailure(): void;
  onCancel(): void;
}

export default function ConfirmPasswordModal({
  onCancel,
  onFailure,
  onSuccess,
}: ConfirmPasswordModalProps) {
  const [opened, { open, close }] = useDisclosure(false);

  useEffect(() => open(), []);

  const form = useForm<ConfirmPasswordRequestBody>({
    validate: schemaResolver(confirmPasswordRequestBodySchema),
  });

  function handleCancel() {
    close();
    onCancel();
  }

  const confirmPassword = useConfirmPassword({ onFailure, onSuccess });

  return (
    <Modal
      title="Confirm Password"
      opened={opened}
      withCloseButton
      onClose={handleCancel}
    >
      <form
        id="confirm-password"
        onSubmit={form.onSubmit(
          async (data) => await confirmPassword.mutateAsync(data),
        )}
      >
        <Stack>
          <PasswordInput
            {...form.getInputProps("password")}
            name="password"
            type="password"
          />
          <Button type="submit" id="confirm-password">
            Submit
          </Button>
        </Stack>
      </form>
    </Modal>
  );
}
