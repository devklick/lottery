import { Group } from "@mantine/core";

import Button from "../../../components/Button";

interface CreateOrEditGameActionButtonsProps {
  cancelDisabled: boolean;
  onCancel(): void;
  submitDisabled: boolean;
  formId: string;
}

export default function CreateOrEditGameActionButtons({
  cancelDisabled,
  onCancel,
  submitDisabled,
  formId,
}: CreateOrEditGameActionButtonsProps) {
  return (
    <Group w={"100%"} justify="end">
      <Button.Pair.CancelSubmit
        cancelDisabled={cancelDisabled}
        onCancelClicked={onCancel}
        submitDisabled={submitDisabled}
        form={formId}
      />
    </Group>
  );
}
