import { Button } from "@mantine/core";
import { RefObject, useImperativeHandle, useState } from "react";

interface DeleteAccountButtonProps {
  ref: RefObject<DeleteAccountButtonRef | null>;
  onClicked(): void;
}

export interface DeleteAccountButtonRef {
  reset(): void;
}

export default function DeleteAccountButton({
  onClicked,
  ref,
}: DeleteAccountButtonProps) {
  const [unlocked, setUnlocked] = useState(false);

  useImperativeHandle(ref, () => ({ reset: () => setUnlocked(false) }), []);

  function handleClick() {
    if (!unlocked) setUnlocked(true);
    else onClicked();
  }
  return (
    <Button
      variant={unlocked ? "filled" : "outline"}
      color="red"
      onClick={handleClick}
    >
      {unlocked ? "Click again to confirm" : "Delete Account"}
    </Button>
  );
}
