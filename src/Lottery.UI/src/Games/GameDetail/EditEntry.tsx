import { CheckIcon, Modal } from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import { useDisclosure } from "@mantine/hooks";

import SelectionPicker from "./SelectionPicker";
import gameService from "../gameService";
import { notifications } from "@mantine/notifications";

interface EditEntryProps {
  selectionNumbers: ReadonlyArray<number>;
  selectedNumbers: ReadonlyArray<number>;
  onClose(): void;
  entryId: string;
}

function EditEntry({
  selectionNumbers,
  selectedNumbers,
  entryId,
  onClose,
}: EditEntryProps) {
  const [opened, { close }] = useDisclosure(true);

  const mutation = useMutation({
    mutationFn: gameService.editEntry,
    onSuccess: () => null,
  });

  async function handleSubmit(selectionNumbers: Array<number>) {
    const id = notifications.show({
      loading: true,
      title: "Updating entry",
      message: `Your numbers are being updated (${selectionNumbers.join(", ")})`,
      autoClose: false,
      allowClose: false,
    });
    await mutation.mutateAsync({
      route: { entryId },
      body: {
        selections: selectionNumbers.map((selectionNumber) => ({
          selectionNumber,
        })),
      },
    });
    notifications.update({
      id,
      loading: false,
      title: "Entry updated",
      message: `Your numbers have been updated (${selectionNumbers.join(", ")})`,
      icon: <CheckIcon />,
      autoClose: 3000,
      allowClose: true,
    });

    close();
    onClose();
  }
  function handleClose() {
    close();
    onClose();
  }
  return (
    <Modal opened={opened} onClose={handleClose}>
      <SelectionPicker
        selectionNumbers={selectionNumbers}
        selectedNumbers={selectedNumbers}
        requiredCount={selectedNumbers.length}
        onSubmit={handleSubmit}
        submitStatus={
          mutation.status === "idle"
            ? "waiting"
            : mutation.status === "pending"
              ? "submitting"
              : "success"
        }
      />
    </Modal>
  );
}

export default EditEntry;
