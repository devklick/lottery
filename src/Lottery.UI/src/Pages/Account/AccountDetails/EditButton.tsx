import { Group, Skeleton } from "@mantine/core";

import Button from "../../../components/Button";

interface EditButtonProps {
  editing: boolean;
  onEditClicked(): void;
  submitEditDisabled: boolean;
  cancelEditDisabled: boolean;
  onCancelEditClicked(): void;
  loading?: boolean;
}

export default function EditButton({
  cancelEditDisabled,
  editing,
  onCancelEditClicked,
  onEditClicked,
  submitEditDisabled,
  loading,
}: EditButtonProps) {
  return (
    <Group justify="flex-end" w="100%">
      {!editing && (
        <Skeleton visible={loading} w={"fit-content"}>
          <Button onClick={onEditClicked}>Edit</Button>
        </Skeleton>
      )}
      {editing && (
        <Button.Pair.CancelSubmit
          cancelDisabled={cancelEditDisabled}
          onCancelClicked={onCancelEditClicked}
          submitDisabled={submitEditDisabled}
          form={"update-account"}
        />
      )}
    </Group>
  );
}
